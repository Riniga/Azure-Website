using System;
using System.IO;
using System.Threading.Tasks;
using AzureWebsite.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureWebApp.Function
{
    public static class MainMenu
    {
        [FunctionName("MainMenu")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger return MainMenu.");
            var menu = new Menu();
            return new OkObjectResult(menu);
        }
    }
}
