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
using Microsoft.WindowsAzure.Storage.Queue;

namespace Skewrl.Core.AzureStorage
{
    public abstract class AzureQueueMessage
    {
        [NonSerialized]
        private IUpdateableAzureQueue updateableQueueReference;

        [NonSerialized]
        private CloudQueueMessage messageReference;

        public CloudQueueMessage GetMessageReference()
        {
            return this.messageReference;
        }

        public IUpdateableAzureQueue GetUpdateableQueueReference()
        {
            return this.updateableQueueReference;
        }

        public void SetMessageReference(CloudQueueMessage reference)
        {
            this.messageReference = reference;
        }

        public void SetUpdateableQueueReference(IUpdateableAzureQueue reference)
        {
            this.updateableQueueReference = reference;
        }

        public void DeleteQueueMessage()
        {
            if (this.updateableQueueReference == null)
            {
                throw new InvalidOperationException("GetUpdateableQueueReference() cannot return null");
            }

            this.updateableQueueReference.DeleteMessage(this);
        }

        public void UpdateQueueMessage()
        {
            if (this.updateableQueueReference == null)
            {
                throw new InvalidOperationException("GetUpdateableQueueReference() cannot return null");
            }

            this.updateableQueueReference.UpdateMessage(this);
        }
    }
}
