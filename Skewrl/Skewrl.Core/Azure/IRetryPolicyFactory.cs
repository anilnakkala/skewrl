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

    public interface IRetryPolicyFactory
    {
        RetryPolicy GetDefaultAzureCachingRetryPolicy();

        RetryPolicy GetDefaultAzureServiceBusRetryPolicy();

        RetryPolicy GetDefaultAzureStorageRetryPolicy();

        RetryPolicy GetDefaultSqlCommandRetryPolicy();

        RetryPolicy GetDefaultSqlConnectionRetryPolicy();
    }
}
