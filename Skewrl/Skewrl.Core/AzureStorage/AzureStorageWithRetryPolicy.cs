//===============================================================================
// Microsoft patterns & practices
// Windows Azure Architecture Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wag.codeplex.com/license)
//===============================================================================


namespace Skewrl.Core.AzureStorage
{
    using System;
    using Microsoft.Practices.TransientFaultHandling;
    using Skewrl.Core.Azure;

    public abstract class AzureStorageWithRetryPolicy : AzureObjectWithRetryPolicyFactory
    {
        protected RetryPolicy StorageRetryPolicy
        {
            get
            {
                var retryPolicy = this.GetRetryPolicyFactoryInstance().GetDefaultAzureStorageRetryPolicy();                    
                retryPolicy.Retrying += new EventHandler<RetryingEventArgs>(RetryPolicyTrace);
                return retryPolicy;
            }
        }
    }
}
