using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Reflection;
using Skewrl.Core.Data;
using Microsoft.Practices.Unity;
using Skewrl.Core.Data.Model;
using Skewrl.Core.Azure;
using Skewrl.Core.AzureStorage;
using System.Threading;

namespace Skewrl.Core.Config
{
    public sealed class SkewrlConfig
    {
        private static SkewrlConfig _Instance = null;

        private CloudStorageAccount _StorageAccount;

        private SkewrlConfig()
        {
           _StorageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString"));            
        }

        public static SkewrlConfig Instance
        {
            get
            {
                if (_Instance != null) return _Instance;

                SkewrlConfig tempObj = new SkewrlConfig();

                Interlocked.CompareExchange(ref _Instance, tempObj, null); 
               
                return _Instance;
            }
        }

        public CloudStorageAccount StorageAccount
        {
            get { return _StorageAccount; }
        }

        
        public void Init()
        {
            InitStorage();
            InitUnity();
        }

        private void InitUnity()
        {
            UnityConfig.Instance.Container.RegisterInstance(SkewrlConfig.Instance.StorageAccount);

            var cloudStorageAccountType = typeof(CloudStorageAccount);
            var retryPolicyFactoryProperty = new InjectionProperty("RetryPolicyFactory", typeof(IRetryPolicyFactory));
            var visibilityTime = TimeSpan.FromSeconds(300);

            //Probably better to register all types that are used across the projects
            //Any types that are used specifically in that role can be be registered there.
            #region Register types for Unity
            UnityConfig.Instance.Container
                .RegisterType<IRetryPolicyFactory, DefaultRetryPolicyFactory>()
                .RegisterType<IAzureObjectWithRetryPolicyFactory, AzureObjectWithRetryPolicyFactory>()
                .RegisterType<IUrlMapDataSource, UrlMapDataSource>()
                .RegisterType<IUrlTrackerDataSource, UrlTrackerDataSource>()
                .RegisterType<IRepository<UrlMap>, AzureTableRepository<UrlMap>>()
                .RegisterType<IRepository<UrlVisited>, AzureTableRepository<UrlVisited>>()
                .RegisterType<IRepository<UrlSummary>, AzureTableRepository<UrlSummary>>()
                .RegisterType<IAzureQueue<UrlRedirectMessage>, AzureQueue<UrlRedirectMessage>>(
                    new InjectionConstructor(cloudStorageAccountType, Constants.AzureQueues.UrlTrackerQueue, visibilityTime),
                    retryPolicyFactoryProperty);
            #endregion
        }

        private void InitStorage()
        {
            #region Init tables
            CloudTableClient tc = _StorageAccount.CreateCloudTableClient();

            IEnumerable<CloudTable> tables = tc.ListTables();

            List<String> tableNames = null;
            foreach (Assembly currentassembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                tableNames = currentassembly.GetTypes()
                           .Where(t => t != null && (t.BaseType != null && t.BaseType.Name == "TableServiceEntity"))
                           .Select(t => t.Name)
                           .ToList();

                //there should be only one assembly with all entities. !!
                //If you have implemented entities in multiple assemblies make appropriate code changes here
                if (tableNames != null && tableNames.Count > 0)
                    break;
            }

            //Create tables
            foreach (String table in tableNames)
            {
                if (tables.FirstOrDefault(c => c.Name.ToUpper() == table.ToUpper()) != null) continue;

                CloudTable cTable = tc.GetTableReference(table.ToLowerInvariant());
                cTable.CreateIfNotExists();
            }

            //Create blob containers
            CloudBlobClient blobClient = _StorageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(Constants.AzureBlobContainer.UrlTrackerContainer);

            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();

            #endregion
        }
    }
}
