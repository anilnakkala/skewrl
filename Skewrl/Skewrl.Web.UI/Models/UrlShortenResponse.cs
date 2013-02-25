using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Skewrl.Web.UI.Models
{
    public class UrlShortenResponse
    {
        public bool Success { get; set; }
        public String Message { get; set; }
        public String Url { get; set; }
        public String QRCodeUrl { get; set; }
    }
}