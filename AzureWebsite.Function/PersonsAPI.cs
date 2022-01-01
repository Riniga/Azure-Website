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
    public static class PersonsAPI
    {
        [FunctionName("GetPersons")]
        public static async Task<IActionResult> GetPersons([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("Return a list of persons");
            var persons = Persons.GetPersons();
            return new OkObjectResult(persons);
        }

        [FunctionName("GetPerson")]
        public static async Task<IActionResult> GetPerson([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            var id = req.Query["personId"];
            log.LogInformation("Return a persons");
            var person = Persons.GetPerson(int.Parse(id));
            return new OkObjectResult(person);
        }

        [FunctionName("CreatePerson")]
        public static async Task<IActionResult> CreatePerson([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            string personJson = await new StreamReader(req.Body).ReadToEndAsync();
            var personObject = JObject.Parse(personJson);

            Console.WriteLine("Subscription:" + personObject);

            Person person = new Person();
            person.Name = (string)personObject["personName"];
            Contract contract = Contracts.GetContract((int)personObject["contractId"]);
            Persons.CreatePerson(person, contract, (int)personObject["amount"]);

            log.LogInformation("Create a new person");

            return new OkObjectResult(true);
        }
        [FunctionName("UpdatePerson")]
        public static async Task<IActionResult> UpdatePerson([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            string personJson = await new StreamReader(req.Body).ReadToEndAsync();
            var personObject = JObject.Parse(personJson);

            if (string.IsNullOrEmpty((string)personObject["personId"]))
            {
                throw new Exception("Unable to update user without ID");
            }

            Person person = new Person();
            person.Name = (string)personObject["personName"];
            person.Id = int.Parse((string)personObject["personId"]);

            Persons.UpdatePerson(person);
            log.LogInformation("Updated person with ID: " + person.Id);
            return new OkObjectResult(true);
        }
    }
}
