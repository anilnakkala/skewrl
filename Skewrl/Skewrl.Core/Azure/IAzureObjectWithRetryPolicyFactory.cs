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
    public interface IAzureObjectWithRetryPolicyFactory
    {
        IRetryPolicyFactory RetryPolicyFactory { get; set; }

        IRetryPolicyFactory GetRetryPolicyFactoryInstance();
    }
}
