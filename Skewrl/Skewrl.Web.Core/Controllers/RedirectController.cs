using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.ApplicationServer.Caching;
using Skewrl.Core.Data;
using Skewrl.Core.Config;
using Skewrl.Core.Data.Model;
using Skewrl.Core.AzureStorage;
using System.Configuration;

namespace Skewrl.Web.Core.Controllers
{
    public class RedirectController : Controller
    {
        DataCache _Cache;
        IUrlMapDataSource _UrlMapDS;
        IUrlTrackerDataSource _UrlTrackerDS;

        public RedirectController()
        {
            _Cache = new DataCache("default");
            _UrlMapDS = UnityConfig.Instance.Resolve<IUrlMapDataSource>();
            _UrlTrackerDS = UnityConfig.Instance.Resolve<IUrlTrackerDataSource>();

        }
        //
        // GET: /Redirect/

        public ActionResult Index(String id)
        {
            // If "id" is null, probably user tried to visit the root of the domain, in which case
            // redirect them to the domain where you would have a landing page
            if(String.IsNullOrEmpty(id))
                Response.Redirect("");
            
            //1. Lookup the key from the cache or Store
            Object urlObj = _Cache.Get(id);
            if (urlObj == null)
            {
                UrlMap map = _UrlMapDS.FindByShortUrlKey(id);
                if (map == null)
                {
                    return new HttpNotFoundResult();
                }

                _Cache.Put(id, map.OriginalUrl);               
                urlObj = map.OriginalUrl;
            }

            //Send a message to the Queue so we can track the request
            UrlRedirectMessage message = new UrlRedirectMessage
            {
                UrlKey=id,
                ActualUrl = urlObj.ToString(),
                AcceptLanguage = HttpContext.Request.Headers["Accept-Language"],
                VisitorIP = HttpContext.Request.UserHostAddress,
                UserAgent = HttpContext.Request.UserAgent,
                Referer = HttpContext.Request.UrlReferrer != null ? HttpContext.Request.UrlReferrer.ToString() : String.Empty
            };

            _UrlTrackerDS.SendUrlVisitedMessage(message);

            //Finally redirect
            return Redirect(urlObj.ToString());
        }

        public RedirectResult GoHome()
        {
            return Redirect(ConfigurationManager.AppSettings["SkewrlUI"]);
        }
    }
}
