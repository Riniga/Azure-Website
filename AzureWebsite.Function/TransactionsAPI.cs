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
    public static class TransactionsAPI
    {
        [FunctionName("GetTransactionsForDebt")]
        public static async Task<IActionResult> GetTransactionsForDebt([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            var id = req.Query["debtId"];
            log.LogInformation("Return a list of transactions for debt with id " + id);
            var transactions = Transactions.GetTransactions(new Debt() { Id = int.Parse(id) } );
            return new OkObjectResult(transactions);
        }

        [FunctionName("CreatePaymentTransaction")]
        public static async Task<IActionResult> CreatePaymentTransaction([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            string paymentObjectJson = await new StreamReader(req.Body).ReadToEndAsync();
            var paymentObject = JObject.Parse(paymentObjectJson);
            Debt debt = new Debt() { Id = ((int)paymentObject["debtId"]) };
            Transactions.CreateTransaction(debt, TransactionType.Payment, (int)paymentObject["amount"]);
            log.LogInformation("Create a new payment for debt with id "+debt.Id );
            return new OkObjectResult(true);
        }

       

    }
}
