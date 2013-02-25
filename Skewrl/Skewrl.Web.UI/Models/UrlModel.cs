using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Skewrl.Core.Data.Model;

namespace Skewrl.Web.UI.Models
{
    public class UrlModel
    {
        public String OriginalUrl { get; set; }     //http://www.somewebsite.com/some_kind_of_a_lengthy_url
        public String ShortUrl { get; set; }        //http://skew.rl/i8U3Hr
        public String ShortUrlCode { get; set; }    //i8U3Hr
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
        public int Clicks { get; set; } //This value must be updated by an aggregator or a worker role periodically

        public static UrlModel From(UrlMap urlMap)
        {
            if (urlMap == null) return new UrlModel();

            return new UrlModel
            {
                OriginalUrl = urlMap.OriginalUrl,
                ShortUrl = urlMap.ShortUrl,
                ShortUrlCode = urlMap.ShortUrlCode,
                IsActive = urlMap.IsActive,
                Clicks=urlMap.Clicks,
                DateCreated = urlMap.DateCreated
            };
        }
    }
}