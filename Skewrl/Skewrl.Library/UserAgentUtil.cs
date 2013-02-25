using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Skewrl.Library
{
    public static class UserAgentUtil
    {
        public static String GetPlatform(String userAgent)
        {
            if (String.IsNullOrEmpty(userAgent))
                return "Others";            

            userAgent = userAgent.ToUpper();

            if (userAgent.IndexOf("WINDOWS") > 0)
                return "Windows";
            else if (userAgent.IndexOf("ANDROID") > 0)
                return "Android";
            else if (userAgent.IndexOf("LINUX") > 0)
                return "Linux";
            else if (userAgent.IndexOf("IPHONE") > 0)
                return "iPhone";
            else if (userAgent.IndexOf("MACINTOSH") > 0 || userAgent.IndexOf("MAC_POWERPC") > 0)
                return "Mac OS";
            else if (userAgent.IndexOf("OS/2") > 0)
                return "OS/2";
            else if (userAgent.IndexOf("SUNOS") > 0)
                return "Sun OS";
            else
                return "Others";

        }

        public static String GetBrowser(String userAgent)
        {
            if (String.IsNullOrEmpty(userAgent))
                return "Others";

            userAgent = userAgent.ToUpper();

            if (userAgent.IndexOf("MSIE") > 0)
                return "Internet Explorer";
            else if (userAgent.IndexOf("FIREFOX") > 0)
                return "Firefox";
            else if (userAgent.IndexOf("CHROME") > 0)
                return "Chrome";
            else if (userAgent.IndexOf("SAFARI") > 0)
                return "Safari";
            else if (userAgent.IndexOf("IPAD") > 0 || userAgent.IndexOf("IPOD") > 0 || userAgent.IndexOf("IPHONE") > 0)
                return "Apple Mobile";
            else if (userAgent.IndexOf("ANDROID") > 0)
                return "Android";
            else if (userAgent.IndexOf("KINDLE") > 0)
                return "Kindle";
            else
                return "Others";

        }
    }
}
