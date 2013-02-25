using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using Skewrl.Web.UI.Models;
using System.Configuration;

namespace Skewrl.Web.UI
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166
            Dictionary<String, object> extraDataMS = new Dictionary<string, object>();
            extraDataMS.Add("icon", "/Images/msn.png");

            OAuthWebSecurity.RegisterMicrosoftClient(
                clientId: ConfigurationManager.AppSettings["MS_Key"],
                clientSecret: ConfigurationManager.AppSettings["MS_Secret"],
                displayName: "Microsoft",
                extraData: extraDataMS
                );

            Dictionary<String, object> extraDataTW = new Dictionary<string, object>();
            extraDataTW.Add("icon", "/Images/twitter.png");

            OAuthWebSecurity.RegisterTwitterClient(
                consumerKey: ConfigurationManager.AppSettings["Twitter_Key"],
                consumerSecret: ConfigurationManager.AppSettings["Twitter_Secret"],
                displayName: "Twitter",
                extraData: extraDataTW
                );

            Dictionary<String, object> extraDataFB = new Dictionary<string, object>();
            extraDataFB.Add("icon", "/Images/facebook.png");

            OAuthWebSecurity.RegisterFacebookClient(
                appId: ConfigurationManager.AppSettings["FB_AppID"],
                appSecret: ConfigurationManager.AppSettings["FB_Secret"],
                displayName: "Facebook",
                extraData: extraDataFB
                );

            Dictionary<String, object> extraDataGL = new Dictionary<string, object>();
            extraDataGL.Add("icon", "/Images/google.png");

            OAuthWebSecurity.RegisterGoogleClient(
                displayName: "Google",
                extraData: extraDataGL
                );
        }
    }
}
