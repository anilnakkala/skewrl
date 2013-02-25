using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.IO;
using Skewrl.Core.Data;
using Skewrl.Core.Config;
using Skewrl.Core.Data.Model;

namespace Skewrl.Web.UI.Controllers
{
    [Authorize]
    public class ChartController : Controller
    {
        public IUrlTrackerDataSource _UrlTrackerDataSource;

        public ChartController()
        {
            _UrlTrackerDataSource = UnityConfig.Instance.Resolve<IUrlTrackerDataSource>();
        }


        public ActionResult Index(String id, int type)
        {
            var summaryList = _UrlTrackerDataSource.GetSummary(id, type);

            //We just need name and value for the chart
            var data = summaryList.Select(u => new
            {
                Name = u.Value,
                Value = u.Count
            });

            return Json(data);
        }

        public ActionResult Timeline(String id)
        {
            var summaryList = _UrlTrackerDataSource.GetSummary(id, 5);

            //Func<UrlSummary, object> dateConf = delegate(UrlSummary url) {
            //    DateTime TempData = DateTime.Parse(url.Value);
            //    return new
            //        {
            //            Name = new DateTime(TempData.Year, TempData.Month, TempData.Day),
            //            Value = url.Count
            //        };
            //};

            //We just need name and value for the chart
            var data = summaryList.Select(u =>
                {
                    return new
                    {
                        Name = DateTime.Parse(u.Value),
                        Value = u.Count
                    };
                });

            return Json(data);
        }

    }
}
