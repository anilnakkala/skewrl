using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.ApplicationServer.Caching;
using Skewrl.Core.Data;
using Skewrl.Core.Config;
using Skewrl.Core.Data.Model;

namespace Skewrl.Web.Core
{
    public class WebRole : RoleEntryPoint
    {
        public override bool OnStart()
        {
            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            SkewrlConfig.Instance.Init();
            return base.OnStart();
        }

        public override void Run()
        {
            // @TODO  
            //May be this code should be moved to global.asax.cs

            //Build the cache here when the web role run is invoked.
            // Create a DataCacheFactoryConfiguration object
            DataCacheFactoryConfiguration config = new DataCacheFactoryConfiguration();

            // Enable the AutoDiscorveryProperty (and any other required configuration settings):
            config.AutoDiscoverProperty = new DataCacheAutoDiscoverProperty(true, "Skewrl.Web.Core");

            // Create a DataCacheFactory object with the configuration settings:
            DataCacheFactory factory = new DataCacheFactory(config);

            // Use the factory to create a DataCache client for the "default" cache:
            DataCache cache = factory.GetCache("default");

            IUrlMapDataSource urlDS = UnityConfig.Instance.Resolve<IUrlMapDataSource>();

            // A good strategy would be load as much you can depending upon the VM and then implement
            // atleast one of the MRU algorithms
            foreach (UrlMap url in urlDS.FindTop100Mappings())
            {               
                cache.Add(url.ShortUrlCode, url.OriginalUrl);
            }

            /*
             * +------------------------------------------------------------------------+
             * | Let this be the last statement.                                        |
             * | Technically Run() is not supposed to return immediately unless as role |
             * | is expected to Run for ever.                                           |
             * | Calling the base.Run() method will ensure this won't be recycled.      |
             * +------------------------------------------------------------------------+
             */
            base.Run();
        }
    }
}
