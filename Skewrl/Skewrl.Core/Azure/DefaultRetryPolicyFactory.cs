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
    using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling.AzureStorage;
    using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling.Cache;
    using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling.ServiceBus;
    using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling.SqlAzure;
    using Microsoft.Practices.TransientFaultHandling;
    

    public class DefaultRetryPolicyFactory : IRetryPolicyFactory
    {
        public RetryPolicy GetDefaultAzureCachingRetryPolicy()
        {
            return new RetryPolicy(new CacheTransientErrorDetectionStrategy(), 3);
        }

        public RetryPolicy GetDefaultAzureServiceBusRetryPolicy()
        {
            return new RetryPolicy(new ServiceBusTransientErrorDetectionStrategy(), 3);
        }

        public RetryPolicy GetDefaultAzureStorageRetryPolicy()
        {
            return new RetryPolicy(new StorageTransientErrorDetectionStrategy(), 3);
        }

        public RetryPolicy GetDefaultSqlCommandRetryPolicy()
        {
            return new RetryPolicy(new SqlAzureTransientErrorDetectionStrategy(), 3);
        }

        public RetryPolicy GetDefaultSqlConnectionRetryPolicy()
        {
            return new RetryPolicy(new SqlAzureTransientErrorDetectionStrategy(), 3);
        }
    }
}
