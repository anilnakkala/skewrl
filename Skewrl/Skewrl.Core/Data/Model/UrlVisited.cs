using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;

namespace Skewrl.Core.Data.Model
{
    public class UrlVisited : TableServiceEntity
    {
        public string UrlKey { get; set; }
        public string ActualUrl { get; set; }

        public string AcceptLanguage { get; set; }
        public string VisitorIP { get; set; }
        public string UserAgent { get; set; }
        public string Referer { get; set; }
        public DateTime DateVisited { get; set; }

        public UrlVisited() 
        {
            long ticks = DateTime.UtcNow.Ticks;

            // Row key allows sorting, so we make sure the rows come back in time order.
            RowKey = string.Format("{0:D19}", DateTime.MaxValue.Ticks - ticks);

            //You should choose partition key using some criteria
            PartitionKey = ticks.ToString();
        }
    }
}
