//===============================================================================
// Microsoft patterns & practices
// Windows Azure Architecture Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wag.codeplex.com/license)
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Skewrl.Core.AzureStorage;

namespace Skewrl.Core.Commands
{
    public interface IBatchCommand<in T> : ICommand<T> where T : AzureQueueMessage
    {
        void PreRun();
        void PostRun();
    }
}
