using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Skewrl.Core.Data.Model;

namespace Skewrl.Web.UI.Models
{
    public class UrlItem
    {
        public String LongUrl { get; set; }     //http://www.somewebsite.com/some_kind_of_a_lengthy_url
        public String ShortUrl { get; set; }        //http://skew.rl/i8U3Hr
        public String ShortUrlCode { get; set; }    //i8U3Hr
        public DateTime Created { get; set; }
        public bool IsActive { get; set; }
        public int Clicks { get; set; } //This value must be updated by an aggregator or a worker role periodically

        public static List<UrlItem> From(List<UrlMap> map)
        {
            if (map == null) return new List<UrlItem>();

            List<UrlItem> urls = new List<UrlItem>();
            foreach (UrlMap url in map)
            {
                urls.Add(new UrlItem
                {
                    LongUrl = url.OriginalUrl,
                    ShortUrl = url.ShortUrl,
                    ShortUrlCode = url.ShortUrlCode,
                    Created = url.DateCreated,
                    IsActive = url.IsActive,
                    Clicks = url.Clicks
                });
            }

            return urls;
        }
    }
    public class UrlListResponse
    {
        public bool Success { get; set; }
        public int Length { get; set; }
        public String Message { get; set; }
        public List<UrlItem> Urls { get; set; }
    }
}