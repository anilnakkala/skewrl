//===============================================================================
// Microsoft patterns & practices
// Windows Azure Architecture Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wag.codeplex.com/license)
//===============================================================================


namespace Skewrl.Core.Azure
{
    using Microsoft.Practices.TransientFaultHandling;
    using Skewrl.Core.Logging;
    using Skewrl.Library.Helpers;

    public abstract class AzureObjectWithRetryPolicyFactory : IAzureObjectWithRetryPolicyFactory
    {
        public IRetryPolicyFactory RetryPolicyFactory { get; set; }

        public virtual IRetryPolicyFactory GetRetryPolicyFactoryInstance()
        {
            return this.RetryPolicyFactory ?? new DefaultRetryPolicyFactory();
        }

        protected virtual void RetryPolicyTrace(object sender, RetryingEventArgs args)
        {
            var msg = string.Format(
                 "{0} Retry - Count:{1}, Delay:{2}, Exception:{3}",
                 this.GetType().Name,
                 args.CurrentRetryCount,
                 args.Delay,
                 args.LastException.TraceInformation());
            TraceHelper.TraceWarning(msg);
        }
    }
}
