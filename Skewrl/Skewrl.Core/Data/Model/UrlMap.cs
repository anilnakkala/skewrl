using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;

namespace Skewrl.Core.Data.Model
{
    public class UrlMap : TableServiceEntity
    {
        public String UserName { get; set; }
        public String OriginalUrl { get; set; }     //http://www.somewebsite.com/some_kind_of_a_lengthy_url
        public String ShortUrl { get; set; }        //http://skew.rl/i8U3Hr
        public String ShortUrlCode { get; set; }    //i8U3Hr
        public DateTime DateCreated { get; set; }
        public DateTime LastAccessed { get; set; }
        public bool IsActive { get; set; }
        public int Clicks { get; set; } //This value must be updated by an aggregator or a worker role periodically

        public UrlMap() 
        {
            long ticks = DateTime.UtcNow.Ticks;

            // Row key allows sorting, so we make sure the rows come back in time order.
            RowKey = string.Format("{0:D19}", DateTime.MaxValue.Ticks - ticks);

            //You should choose partition key using some criteria
            PartitionKey = ticks.ToString();
        }
    }
}
