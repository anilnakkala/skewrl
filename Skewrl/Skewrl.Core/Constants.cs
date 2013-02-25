using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skewrl.Core
{
    public static class Constants
    {
        //Queue Names
        public static class AzureQueues
        {
            public const string UrlTrackerQueue = "urltrackerqueue";
            public const string UrlGeneratorQueue = "urlgeneratorqueue";
        }

        //Blob Container Names
        public static class AzureBlobContainer
        {
            public const string UrlTrackerContainer = "urlqrimages";
        }

        public static class SummaryType
        {
            public const int Referer = 1;
            public const int Browser = 2;
            public const int Platform = 3;
            public const int Country = 4;
            public const int Clicks = 5;
        }
    }
}
