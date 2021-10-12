using System.Threading.Tasks;
using AzureWebsite.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace AzureWebApp.Function
{
    public static class MainMenu
    {
        [FunctionName("GetCosmosDbMenu")]
        public static async Task<IActionResult> GetCosmosDbMenu([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("Return a menu tree");
            Menu root = await CosmosHelper.GetMenuFromCosmosDb();
            return new OkObjectResult(root);
        }


        [FunctionName("CreateCosmosDbMenu")]
        public static async Task<IActionResult> CreateCosmosDbMenu([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("Create a new menu to Cosmos DB ");
            await CosmosHelper.CreateMenuInCosmosDb();
            return new OkObjectResult("{\"Result\":\"Meu Created\"}");
        }
    }
}
