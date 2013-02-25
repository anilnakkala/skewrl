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

    public class PessimisticConcurrencyContext : IConcurrencyControlContext
    {
        public PessimisticConcurrencyContext()
        {
            this.Duration = TimeSpan.FromSeconds(30);
        }

        public string LockId { get; internal set; }

        public string ObjectId { get; set; }

        public TimeSpan Duration { get; set; }
    }
}
