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
    using System.Collections.Generic;
    using System.Web.Script.Serialization;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Queue;
    using Microsoft.WindowsAzure.Storage.RetryPolicies;
 

    public class AzureQueue<T> : AzureStorageWithRetryPolicy, IAzureQueue<T>, IUpdateableAzureQueue
        where T : AzureQueueMessage
    {
        private readonly CloudStorageAccount account;
        private readonly TimeSpan visibilityTimeout;
        private readonly CloudQueue queue;

        public AzureQueue(CloudStorageAccount account)
            : this(account, typeof(T).Name.ToLowerInvariant())
        {
        }

        public AzureQueue(CloudStorageAccount account, string queueName)
            : this(account, queueName, TimeSpan.FromSeconds(30))
        {
        }

        public AzureQueue(CloudStorageAccount account, string queueName, TimeSpan visibilityTimeout)
        {
            this.account = account;
            this.visibilityTimeout = visibilityTimeout;

            var client = this.account.CreateCloudQueueClient();

            // retry policy is handled by TFHAB
            client.RetryPolicy = new NoRetry();

            this.queue = client.GetQueueReference(queueName.ToLowerInvariant());
        }

        public void AddMessage(T message)
        {
            this.StorageRetryPolicy.ExecuteAction(() => this.queue.AddMessage(new CloudQueueMessage(GetSerializedMessage(message))));
        }

        public void DeleteMessage(AzureQueueMessage message)
        {
            if (!(message is T))
            {
                throw new ArgumentException("Message should be instance of T", "message");
            }

            this.DeleteMessage(message as T);
        }

        public void UpdateMessage(AzureQueueMessage message)
        {
            if (!(message is T))
            {
                throw new ArgumentException("Message should be instance of T", "message");
            }

            var messageRef = message.GetMessageReference();
            if (messageRef == null)
            {
                throw new ArgumentException("Message reference cannot be null", "message.GetMessageReference()");
            }

            this.StorageRetryPolicy.ExecuteAction(() =>
                {
                    messageRef.SetMessageContent(GetSerializedMessage(message as T));
                    this.queue.UpdateMessage(messageRef, this.visibilityTimeout, MessageUpdateFields.Visibility | MessageUpdateFields.Content);
                });
        }

        public T GetMessage()
        {
            var message = this.StorageRetryPolicy.ExecuteAction<CloudQueueMessage>(() => this.queue.GetMessage(this.visibilityTimeout));

            if (message == null)
            {
                return default(T);
            }

            return GetDeserializedMessage(this, message);
        }

        public IEnumerable<T> GetMessages(int maxMessagesToReturn)
        {
            var messages = this.StorageRetryPolicy.ExecuteAction<IEnumerable<CloudQueueMessage>>(() => this.queue.GetMessages(maxMessagesToReturn, this.visibilityTimeout));

            foreach (var message in messages)
            {
                yield return GetDeserializedMessage(this, message);
            }
        }

        public void EnsureExist()
        {
            this.StorageRetryPolicy.ExecuteAction(() => this.queue.CreateIfNotExists());
        }

        public void Clear()
        {
            this.StorageRetryPolicy.ExecuteAction(() => this.queue.Clear());
        }

        public void DeleteMessage(T message)
        {
            var messageRef = message.GetMessageReference();
            if (messageRef == null)
            {
                throw new ArgumentException("Message reference cannot be null", "message.GetMessageReference()");
            }

            this.StorageRetryPolicy.ExecuteAction(() => this.queue.DeleteMessage(messageRef.Id, messageRef.PopReceipt));
        }

        private static string GetSerializedMessage(T message)
        {
            return new JavaScriptSerializer().Serialize(message);
        }

        private static T GetDeserializedMessage(IUpdateableAzureQueue queue, CloudQueueMessage message)
        {
            var deserializedMessage = new JavaScriptSerializer().Deserialize<T>(message.AsString);

            // set references (allows updating message)
            deserializedMessage.SetUpdateableQueueReference(queue);
            deserializedMessage.SetMessageReference(message);

            return deserializedMessage;
        }
    }
}