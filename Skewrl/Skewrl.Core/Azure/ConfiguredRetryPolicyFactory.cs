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
    using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling;
    using Microsoft.Practices.TransientFaultHandling;

    public class ConfiguredRetryPolicyFactory : IRetryPolicyFactory
    {
        public RetryPolicy GetDefaultAzureCachingRetryPolicy()
        {
            return RetryPolicyFactory.GetDefaultAzureCachingRetryPolicy();
        }

        public RetryPolicy GetDefaultAzureServiceBusRetryPolicy()
        {
            return RetryPolicyFactory.GetDefaultAzureServiceBusRetryPolicy();
        }

        public RetryPolicy GetDefaultAzureStorageRetryPolicy()
        {
            return RetryPolicyFactory.GetDefaultAzureStorageRetryPolicy();
        }

        public RetryPolicy GetDefaultSqlCommandRetryPolicy()
        {
            return RetryPolicyFactory.GetDefaultSqlCommandRetryPolicy();
        }

        public RetryPolicy GetDefaultSqlConnectionRetryPolicy()
        {
            return RetryPolicyFactory.GetDefaultSqlConnectionRetryPolicy();
        }
    }
}
