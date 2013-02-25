using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skewrl.Library.GeoLite;
using System.IO;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Threading;

namespace Skewrl.Core.Services
{
    public class LocationService
    {
        private static LocationService _instance = null;
        private LookupService _LookupService;

        private LocationService()
        {
            string appRoot = Environment.GetEnvironmentVariable("RoleRoot");

            appRoot = Path.Combine(appRoot + @"\", @"approot\App_Data\GeoLiteCity.dat");
            _LookupService = new LookupService(appRoot, LookupService.GEOIP_STANDARD);

        }

        public static LocationService Instance
        {
            get
            {
                if (_instance != null) return _instance;

                LocationService tempObj = new LocationService();

                Interlocked.CompareExchange(ref _instance, tempObj, null);

                return _instance;
            }
        }

        public Location GetLocationByIp(String IpAddress)
        {
            Location l = _LookupService.getLocation(IpAddress);

            return l;
        }
    }
}
