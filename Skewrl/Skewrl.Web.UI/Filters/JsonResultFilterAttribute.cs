using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Skewrl.Web.UI.Filters
{
    public class JsonResultFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var useragent = HttpContext.Current.Request.UserAgent;

            if(!String.IsNullOrEmpty(useragent) && useragent.ToLower().Contains("msie"))
                HttpContext.Current.Response.ContentType = "text/html";
            else
                HttpContext.Current.Response.ContentType = "application/json";

        }
    }
}