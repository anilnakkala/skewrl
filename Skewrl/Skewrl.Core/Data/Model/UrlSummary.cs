using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;

namespace Skewrl.Core.Data.Model
{
    public class UrlSummary : TableServiceEntity
    {
        public string UrlKey { get; set; }
        public string Value { get; set; }
        public int Count { get; set; }
        public int SummaryType { get; set; }
        public UrlSummary() 
        {
            long ticks = DateTime.UtcNow.Ticks;

            // Row key allows sorting, so we make sure the rows come back in time order.
            RowKey = string.Format("{0:D19}", ticks);

            //You should choose partition key using some criteria
            PartitionKey = ticks.ToString();
        }
    }
}
