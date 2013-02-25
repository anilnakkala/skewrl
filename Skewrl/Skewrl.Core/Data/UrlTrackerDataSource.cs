using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skewrl.Core.Data.Model;
using Skewrl.Core.AzureStorage;
using Skewrl.Core.Services;
using Skewrl.Library.GeoLite;
using Skewrl.Library;

namespace Skewrl.Core.Data
{
    public interface IUrlTrackerDataSource
    {
        void UpdateVisitData(UrlVisited url);
        void SendUrlVisitedMessage(UrlRedirectMessage message);
        List<UrlSummary> GetSummary(String urlKey, int summaryType);
    }

    public class UrlTrackerDataSource : IUrlTrackerDataSource
    {
        private IRepository<UrlVisited> _UrlRepository;
        private IRepository<UrlSummary> _UrlRefRepository;
        private IAzureQueue<UrlRedirectMessage> _TrackerQueue;

        public UrlTrackerDataSource(IRepository<UrlVisited> UrlRepository, 
            IAzureQueue<UrlRedirectMessage> TrackerQueue,
            IRepository<UrlSummary> UrlRefRepository)
        {
            _UrlRepository = UrlRepository;
            _UrlRefRepository = UrlRefRepository;
            _TrackerQueue = TrackerQueue;
            _TrackerQueue.EnsureExist();
        }  
        
        public List<UrlSummary> GetSummary(String urlKey, int summaryType)
        {
            return _UrlRefRepository.FindAll(u => u.UrlKey == urlKey && u.SummaryType == summaryType);
        }

        public void UpdateVisitData(UrlVisited url)
        {
            _UrlRepository.Add(url);

            #region Update Referer Summary            
            //Update referral count summary
            //If the referer is empty, save it as Unknown, else try to get the domain name
            String referer = "unknown";
            if (!String.IsNullOrEmpty(url.Referer))
            {
                var uri = new Uri(url.Referer);
                referer = uri.GetLeftPart(UriPartial.Authority);

                if (!String.IsNullOrEmpty(referer))
                {
                    //just remove the protocol
                    int index = referer.IndexOf(":");
                    if (index > 0)
                    {
                        referer = referer.Substring(index + 3);
                    }
                }
                else
                    referer = "others";

            }

            UpdateSummary(url.UrlKey, referer, Constants.SummaryType.Referer);
            #endregion

            #region Update Country Summary
            if (!String.IsNullOrEmpty(url.VisitorIP))
            {
                Location loc = LocationService.Instance.GetLocationByIp(url.VisitorIP);

                if (loc != null)
                    UpdateSummary(url.UrlKey, loc.countryName, Constants.SummaryType.Country);
            }
            #endregion

            #region Browser name
            UpdateSummary(url.UrlKey, UserAgentUtil.GetBrowser(url.UserAgent), Constants.SummaryType.Browser);            
            #endregion

            #region Platform name
            UpdateSummary(url.UrlKey, UserAgentUtil.GetPlatform(url.UserAgent), Constants.SummaryType.Platform);
            #endregion

            #region Increment click counter
            UpdateSummary(url.UrlKey, DateTime.UtcNow.ToString(), Constants.SummaryType.Clicks);
            #endregion
        }

        private void UpdateSummary(String urlKey, String sValue, int sType )
        {
            UrlSummary refSum = _UrlRefRepository.FindSingle(u =>
                u.UrlKey == urlKey &&
                u.Value == sValue &&
                u.SummaryType == sType);

            if (refSum != null)
            {
                ++refSum.Count;
                _UrlRefRepository.Update(refSum);
            }
            else
            {
                refSum = new UrlSummary();
                refSum.Count = 1;
                refSum.UrlKey = urlKey;
                refSum.Value = sValue;
                refSum.SummaryType = sType;
                _UrlRefRepository.Add(refSum);
            }
        }

        public void SendUrlVisitedMessage(UrlRedirectMessage message)
        {
            _TrackerQueue.AddMessage(message);
        }
    }
}
