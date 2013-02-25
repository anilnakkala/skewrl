using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skewrl.Core.Data.Model;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Skewrl.Core.Config;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace Skewrl.Core.Data
{
    public interface IUrlMapDataSource
    {
        void AddUrlMap(UrlMap url, String userName);
        void RemoveUrlMap(UrlMap url, String userName);
        void RemoveUrlMap(string ShortUrl, String UserName);
       // void RemoveUserUrlMap(String id, String userName);
        void UpdateMap(UrlMap url);
        void ChangeUrlStatus(String id, bool bActive);

        UrlMap FindByShortUrlKey(String id);
        UrlMap FindByActualUrl(String Url);
        List<UrlMap> FindTop100Mappings();

        List<UrlMap> FindAllUrlsByUserName(String userName);
        UrlMap FindSingleUrlByUserName(String Url, String userName);

        void SaveQRCode(String file, String name);
        void GetQRCodeToFile(String file, String name);
    }

    public class UrlMapDataSource : IUrlMapDataSource
    {
        private IRepository<UrlMap> _UrlRepository;
        private IRepository<UrlSummary> _UrlSummaryRepository;
        private readonly CloudBlobContainer _Container;

        public UrlMapDataSource(IRepository<UrlMap> UrlRepository, IRepository<UrlSummary> UrlSummaryRepository)
        {
            _UrlRepository = UrlRepository;
            _UrlSummaryRepository = UrlSummaryRepository;

            CloudStorageAccount account = UnityConfig.Instance.Resolve<CloudStorageAccount>();

            var client = account.CreateCloudBlobClient();
            client.RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(5), 3);

            //Container would be created during role startup
            _Container = client.GetContainerReference(Constants.AzureBlobContainer.UrlTrackerContainer);
        }

        public UrlMap FindByShortUrlKey(String id)
        {
            return _UrlRepository.FindFirst(u => u.ShortUrlCode == id);
        }

        public UrlMap FindByActualUrl(String Url)
        {
            return _UrlRepository.FindFirst(u => u.OriginalUrl == Url);
        }

        public void AddUrlMap(UrlMap url, String userName = null)
        {
            url.UserName = userName;
            _UrlRepository.Add(url);
        }

        public void RemoveUrlMap(string ShortUrl, String UserName)
        {
            UrlMap map = null;

            if (!String.IsNullOrEmpty(UserName))
                map = _UrlRepository.FindSingle(u => u.ShortUrlCode == ShortUrl && u.UserName == UserName);
            else
                map = _UrlRepository.FindSingle(u => u.ShortUrlCode == ShortUrl);

            if (map != null)
                _UrlRepository.Delete(map);
        }

        public void RemoveUrlMap(UrlMap url, String userName = null)
        {
            _UrlRepository.Delete(url);
        }

        public void ChangeUrlStatus(String id, bool bActive)
        {
            UrlMap map = _UrlRepository.FindSingle(u => u.ShortUrlCode == id);

            if (map != null)
            {
                map.IsActive = bActive;
                UpdateMap(map);
            }
        }

        public void UpdateMap(UrlMap url)
        {
            _UrlRepository.Update(url);
        }

        public void SaveQRCode(String file, String blobName)
        {
            blobName = blobName + ".qr";

            CloudBlockBlob blob = _Container.GetBlockBlobReference(blobName);

            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                blob.UploadFromStream(fs);
            }
        }

        public void GetQRCodeToFile(String file, String name)
        {
            CloudBlockBlob blob = _Container.GetBlockBlobReference(name);

            if (blob == null)
                throw new Exception("Blob " + name + " doesn't exist");

            using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate))
            {
                blob.DownloadToStream(fs);
            }
        }

        public List<UrlMap> FindTop100Mappings()
        {
            return _UrlRepository.FindN(null, 100);
        }

        public UrlMap FindSingleUrlByUserName(String Url, String userName)
        {
            UrlMap urlMap = _UrlRepository.FindSingle(u => u.OriginalUrl == Url && u.UserName == userName);

           return urlMap;
        }

        public List<UrlMap> FindAllUrlsByUserName(String userName)
        {
            List<UrlMap> urlMapList = _UrlRepository.FindAll(u => u.UserName == userName);

            foreach (UrlMap map in urlMapList)
            {
                UrlSummary summ = _UrlSummaryRepository.FindSingle(u => u.UrlKey == map.ShortUrlCode && u.SummaryType == 5);

                if (summ != null)
                {
                    map.Clicks = summ.Count;
                }
            }
            
            return urlMapList;
        }
    }
}
