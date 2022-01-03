using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AzureWebApp.Function
{
    public static class ContractsAPI
    {
        [FunctionName("GetContracts")]
        public static async Task<IActionResult> GetContracts([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("Return a list of contracts");
            var contracts = await AzureWebsite.Library.Inkasso.Contracts.GetContractsAsync();
            return new OkObjectResult(contracts);
        }

        [FunctionName("GetContract")]
        public static async Task<IActionResult> GetContract([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            var id = req.Query["contractId"];
            log.LogInformation("Return a contract");
            var contract = await AzureWebsite.Library.Inkasso.Contracts.GetContractAsync(int.Parse(id));
            return new OkObjectResult(contract);
        }
    }
}
