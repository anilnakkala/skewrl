using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Skewrl.Web.UI.Filters;
using Gma.QrCodeNet.Encoding;
using System.Net;
using System.Net.Cache;
using Skewrl.Web.UI.Models;
using Gma.QrCodeNet.Encoding.Windows.Controls;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using Skewrl.Core.Data;
using Microsoft.Practices.Unity;
using Skewrl.Library;
using Skewrl.Core.Config;
using System.Configuration;
using Skewrl.Core.Data.Model;
using System.Web.Script.Serialization;

namespace Skewrl.Web.UI.Controllers
{
    public class HomeController : Controller
    {
        private String _URLShortenerHost;
        public IUrlMapDataSource _UrlMapDataSource;

        public HomeController()
        {
            _UrlMapDataSource = UnityConfig.Instance.Resolve<IUrlMapDataSource>();
            _URLShortenerHost = ConfigurationManager.AppSettings["URLShortenerHost"];
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Generate(String longurl)
        {
            var jsonResp = new UrlShortenResponse{
            Success = true
            };

            /*
             *   +--------------------------------------------------------------------------------+
             *   | Step 1: Check if the URL is valid. We will issue a web request and see the     |
             *   |         status code.                                                           |
             *   | Step 2: Check if this URL is already in the system. If exists, return          |
             *   |         We will check for the current user, since we will generate a unique    |
             *   |         URL for each user for a given original URL                             |
             *   | Step 3: Generate short URL. (May be check again if we generated a duplicate?)  |
             *   | Step 4: Generate a QR code for the URL                                         |
             *   | Step 5: Save generated short url and also save the QR image.                   |
             *   | Step 6: Send Json response back to the user.                                   |
             *   +--------------------------------------------------------------------------------+
            */

            #region Step 1 - Check for a valid URL

            bool bValidUrl = true;
            try
            {
                HttpWebRequest request = WebRequest.Create(longurl) as HttpWebRequest;
                request.Method = "GET";
                request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                var response = request.GetResponse();
                bValidUrl = response.Headers.Count > 0;
            }
            catch
            {
                bValidUrl = false;
            }

            if (!bValidUrl)
            {
                return Json(new UrlShortenResponse
                {
                    Success = false,
                    Message = "Please check if the URL is correct and try again." 
                }, "text/html");
            }

            String protoPrefix = "http://";
            String webUrl = longurl;
            if (longurl.Contains("://"))
            {
                int iIndex = longurl.IndexOf("://");
                protoPrefix = longurl.Substring(0, iIndex + 3);
                webUrl = longurl.Substring(iIndex + 3);
            }

            #endregion

            #region Step 2-5 - Generate URL and save

            UrlMap url = _UrlMapDataSource.FindSingleUrlByUserName(longurl, User.Identity.Name);
            if (url == null)
            {
                url = new UrlMap();
                url.OriginalUrl = longurl;
                url.DateCreated = DateTime.UtcNow;
                url.IsActive = true;

                //Step 3:
                //Now generate hash for the longUrl;
                uint hash = FNVHash.fnv_32a_str(webUrl + User.Identity.Name);

                //Convert hash to base36
                url.ShortUrlCode = Base36Converter.Encode(hash);

                url.ShortUrl = String.Format("{0}{1}", _URLShortenerHost, url.ShortUrlCode);

                //Step 4:
                QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
                QrCode qrCode = new QrCode();
                qrEncoder.TryEncode(url.ShortUrl, out qrCode);

                Renderer renderer = new Renderer(5, Brushes.Black, Brushes.White);

                String fileName = String.Format("{0}{1}.qr", AppDomain.CurrentDomain.BaseDirectory, url.ShortUrlCode);
                renderer.CreateImageFile(qrCode.Matrix, fileName, ImageFormat.Png);
                jsonResp.QRCodeUrl = String.Format("{0}Home/QRImage/{1}.qr", _URLShortenerHost, url.ShortUrlCode);

                //Save Url Map
                _UrlMapDataSource.AddUrlMap(url, User.Identity.Name);

                //Save method appends the .qr extension
                _UrlMapDataSource.SaveQRCode(fileName, url.ShortUrlCode);
            }
            #endregion
           
            // Step 6
            jsonResp.Url = url.ShortUrl;
            jsonResp.QRCodeUrl = String.Format("{0}Home/QRImage/{1}.qr", _URLShortenerHost, url.ShortUrlCode);
                        
            return Json(jsonResp, "text/html");
        }

        public FileStreamResult QRImage(String id)
        {
            var path = String.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, id);

            if (!System.IO.File.Exists(path))
            {
                //Get from the blob
                _UrlMapDataSource.GetQRCodeToFile(path, id);
            }

            return new FileStreamResult(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read), "image/png");
        }

        [Authorize]
        public ActionResult MyUrlsJson()
        {

            List<UrlMap> urls = _UrlMapDataSource.FindAllUrlsByUserName(User.Identity.Name);
            if (urls != null && urls.Count > 0)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                String s = serializer.Serialize(new UrlListResponse { Success = true, Urls = UrlItem.From(urls), Length = urls.Count });

                return Json(
                new UrlListResponse { Success = true, Urls = UrlItem.From(urls), Length = urls.Count },
                "text/html", JsonRequestBehavior.AllowGet);
            }
            else
                return Json(
                new UrlListResponse { Message = "No Urls found" },
                "text/html", JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult MyUrls()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ActionName("MyUrl")]
        public String UpdateUrl(String id, bool flag)
        {
            _UrlMapDataSource.ChangeUrlStatus(id, flag);

            return "{\"message\":\"Successfully updated.\"}";
        }

        [Authorize]
        [HttpDelete]
        [ActionName("MyUrl")]
        public String DeleteUrl(String id)
        {
            _UrlMapDataSource.RemoveUrlMap(id, User.Identity.Name);

            return "{\"message\":\"Successfully deleted.\"}";
        }

        [Authorize]
        [ActionName("MyUrl")]
        public ActionResult Analytics(String id)
        {
            UrlMap map = _UrlMapDataSource.FindByShortUrlKey(id);

            if (map == null)
            {
                return null;
            }

            UrlModel url = UrlModel.From(map);

            return View("Analytics", url);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
