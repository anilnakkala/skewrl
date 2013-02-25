using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Skewrl.Core.Config;
using Skewrl.Core.Azure;
using Microsoft.Practices.Unity;
using Skewrl.Core.AzureStorage;
using Skewrl.Core.QueueHandlers;
using Skewrl.Worker.Commands;

namespace Skewrl.Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        // No further implementation is required to support a cache worker role. 
        // Additional functionality may affect the performance of the cache service. 
        // For information on the dedicated cache worker role and the cache service 
        // see the MSDN documentation at http://go.microsoft.com/fwlink/?LinkID=247285
        public override bool OnStart()
        {
            SkewrlConfig.Instance.Init();

            // http://msdn.microsoft.com/en-us/library/hh680900(v=pandp.50).aspx
            UnityConfig.Instance.Container.RegisterInstance<IRetryPolicyFactory>(new DefaultRetryPolicyFactory() as IRetryPolicyFactory);
            
            return base.OnStart();
        }

        public override void Run()
        {
            var trackerQueue = UnityConfig.Instance.Container.Resolve<IAzureQueue<UrlRedirectMessage>>();

            QueueHandler
                .For(trackerQueue)
                .Every(TimeSpan.FromSeconds(5))
                .Do(UnityConfig.Instance.Resolve<UrlRedirectTrackerCommand>());

            while (true)
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }
    }
}
