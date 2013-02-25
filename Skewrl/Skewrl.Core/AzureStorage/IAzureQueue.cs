//===============================================================================
// Microsoft patterns & practices
// Windows Azure Architecture Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wag.codeplex.com/license)
//===============================================================================

using System.Collections.Generic;

namespace Skewrl.Core.AzureStorage
{
    public interface IAzureQueue<T> where T : AzureQueueMessage
    {
        void EnsureExist();
        void Clear();
        void AddMessage(T message);
        T GetMessage();
        IEnumerable<T> GetMessages(int maxMessagesToReturn);
        void DeleteMessage(T message);
    }
}
