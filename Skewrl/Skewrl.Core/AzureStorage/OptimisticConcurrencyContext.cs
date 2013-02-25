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
    using Microsoft.WindowsAzure.Storage;

    public class OptimisticConcurrencyContext : IConcurrencyControlContext
    {
        public OptimisticConcurrencyContext()
        {
           // this.AccessCondition = AccessCondition.None; <<Before v2 SDK >>
            this.AccessCondition = AccessCondition.GenerateEmptyCondition();
        }

        internal OptimisticConcurrencyContext(string entityTag)
        {
            this.AccessCondition = AccessCondition.GenerateIfMatchCondition(entityTag);
        }

        public AccessCondition AccessCondition { get; set; }

        public string ObjectId { get; set; }
    }
}
