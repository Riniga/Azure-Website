using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using AzureWebsite.Library;
using System.Linq;

namespace AzureFunction
{
    public static class Blobs
    {
        [FunctionName("ListAllBlobs")]
        public static async Task<IActionResult> ListAllBlobs([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,ILogger log)
        {
            log.LogInformation("List all blobs in storage given in connection string and container images");
            
            var blobs =  await BlobManager.RetrieveContainerBlobs(Environment.GetEnvironmentVariable("StorageConnectionString"), "images");
            var imageList = blobs.Select(blob => new[] { blob.Name, blob.Uri.ToString() } ).ToList();

            return new OkObjectResult(imageList);
        }

        [FunctionName("UploadBlobItem")]
        public static async Task<IActionResult> UploadBlobItem([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            var name = req.Query["name"];
            var blob = req.Body;

            log.LogInformation("Save an uploade image with name: " + name);

            await BlobManager.UploadBlob(name, "image/jpg", blob, Environment.GetEnvironmentVariable("StorageConnectionString"), "images");
            return new OkObjectResult("Image saved");
        }
    }
}
