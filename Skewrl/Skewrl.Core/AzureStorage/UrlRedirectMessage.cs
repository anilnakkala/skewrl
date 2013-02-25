using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skewrl.Core.AzureStorage
{
    public class UrlRedirectMessage : AzureQueueMessage
    {
        public string UrlKey { get; set; }
        public string ActualUrl { get; set; }

        // see this URL for definitions of the following fields. 
        // http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html

        public string AcceptLanguage { get; set; }
        public string VisitorIP { get; set; }
        public string UserAgent { get; set; }
        public DateTime DateVisited { get; set; }
        public string Referer { get; set; }
    }
}
