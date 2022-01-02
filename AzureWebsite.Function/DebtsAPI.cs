using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using AzureWebsite.Library.Inkasso;
using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AzureWebApp.Function
{
    public static class DebtsAPI
    {
        [FunctionName("GetPersonDebts")]
        public static async Task<IActionResult> GetPersonDebts([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            var id = req.Query["personId"];
            log.LogInformation("Return a list of debts for a person");
            var debts = Debts.GetDebts(new Person() { Id = int.Parse(id) } );
            return new OkObjectResult(debts);
        }

        [FunctionName("GetDebt")]
        public static async Task<IActionResult> GetDebt([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            var id = req.Query["debtId"];
            log.LogInformation("Return a debt with id " +id);
            var debt = Debts.GetDebt(int.Parse(id));
            return new OkObjectResult(debt);
        }

        [FunctionName("CreateDebt")]
        public static async Task<IActionResult> CreateDebt([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            string personJson = await new StreamReader(req.Body).ReadToEndAsync();
            var personObject = JObject.Parse(personJson);

            Console.WriteLine("Subscription:" + personObject);

            Person person = new Person() { Id = (int)personObject["personId"] };
            Contract contract = Contracts.GetContract((int)personObject["contractId"]);
            Debts.CreateDebt(person, contract, (int)personObject["amount"]);

            log.LogInformation("Create a new Debt for person with id "+person.Id );

            return new OkObjectResult(true);
        }
    }
}
