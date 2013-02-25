using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;

namespace Skewrl.Core.Extensions
{
    public static class AzureExtensions
    {
        /// <summary>
        /// DownloadByteArray has been removed from Azure 2.0 SDK.
        /// Adding a method here for backward compatibility
        /// </summary>
        /// <param name="blob"></param>
        /// <returns></returns>
        public static byte[] DownloadByteArray(this ICloudBlob blob)
        {
            using (var ms = new MemoryStream())
            {
                blob.DownloadToStream(ms);
                ms.Position = 0;
                return ms.ToArray();
            }
        }

        /// <summary>
        /// UploadByteArray has been removed from Azure 2.0 SDK.
        /// Adding a method here for backward compatibility
        /// </summary>
        /// <param name="blob"></param>
        /// <param name="data"></param>
        /// <param name="options"></param>
        public static void UploadByteArray(this ICloudBlob blob, byte[] data, BlobRequestOptions options)
        {
            using (var ms = new MemoryStream(data))
            {
                blob.UploadFromStream(ms, null, options);
            }
        }
    }
}
