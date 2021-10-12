using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace AzureWebsite.Library
{
    public class BlobManager
    {
        public static async Task<List<CloudBlockBlob>> RetrieveContainerBlobs(string connectionString, string containerReference)
        {
            CloudBlobContainer container = await GetBlobContainer(connectionString, containerReference);
            var blobs = new List<CloudBlockBlob>();
            BlobContinuationToken continuationToken = null;
            do
            {
                var response = await container.ListBlobsSegmentedAsync(continuationToken);
                continuationToken = response.ContinuationToken;
                blobs.AddRange(from CloudBlockBlob blob in response.Results select blob);
            }
            while (continuationToken != null);
            return blobs;
        }
        public static async Task UploadBlob(string name, string contentType, System.IO.Stream blob, string connectionString, string containerReference)
        {
            CloudBlobContainer container = await GetBlobContainer(connectionString, containerReference);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(name);
            blockBlob.Properties.ContentType = "image/jpg";
            await blockBlob.UploadFromStreamAsync(blob);
        }
        private static async Task<CloudBlobContainer> GetBlobContainer(string connectionString, string containerReference)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(containerReference);
            var requestOptions = new BlobRequestOptions() { RetryPolicy = new NoRetry() };
            await container.CreateIfNotExistsAsync(requestOptions, null);
            return container;
        }
    }
}
