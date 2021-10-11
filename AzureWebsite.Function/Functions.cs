using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using System.Collections.Generic;

namespace AzureFunction
{
    public static class Functions
    {
        [FunctionName("ListAllBlobs")]
        public static async Task<IActionResult> ListAllBlobs([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,ILogger log)
        {
            log.LogInformation("List all blobs in storage given in connection string and container images");
            var blobs = new List<string[]>();
            var connectionString = Environment.GetEnvironmentVariable("StorageConnectionString");
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("images");
            var requestOptions = new BlobRequestOptions() { RetryPolicy = new NoRetry() };
            await container.CreateIfNotExistsAsync(requestOptions, null);
            BlobContinuationToken continuationToken = null;
            do
            {
                var response = await container.ListBlobsSegmentedAsync(continuationToken);
                continuationToken = response.ContinuationToken;
                foreach (CloudBlockBlob blob in response.Results)
                {
                    blobs.Add(new[] { blob.Name,  blob.Uri.ToString() });
                }
            }
            while (continuationToken != null);
            return new OkObjectResult(blobs);
        }

        [FunctionName("UploadBlobItem")]
        public static async Task<IActionResult> UploadBlobItem([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            var name = req.Query["name"];
            log.LogInformation("Save an uploade image with name: " + name);
            
            var connectionString = Environment.GetEnvironmentVariable("StorageConnectionString");
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("images");

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(name);
            blockBlob.Properties.ContentType = "image/jpg";

            await blockBlob.UploadFromStreamAsync(req.Body);
            return new OkObjectResult("Image saved");
        }

    }
}
