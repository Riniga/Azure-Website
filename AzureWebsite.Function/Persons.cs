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
    public static class Persons
    {
        [FunctionName("GetPersons")]
        public static async Task<IActionResult> GetPersons([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("Return a list of persons");
            var persons = PersonManager.GetPersons();
            return new OkObjectResult(persons);
        }

        [FunctionName("GetPerson")]
        public static async Task<IActionResult> GetPerson([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            var id = req.Query["id"];
            log.LogInformation("Return a persons");
            var person = PersonManager.GetPerson(System.Guid.Parse(id));
            return new OkObjectResult(person);
        }

        [FunctionName("SavePerson")]
        public static async Task<IActionResult> SavePerson([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            string personJson = await new StreamReader(req.Body).ReadToEndAsync();
            var personObject = JObject.Parse(personJson);

            Console.WriteLine("Subscription:" + personObject);

            Person person = new Person();
            person.Name = (string)personObject["name"];

            if (!string.IsNullOrEmpty((string)personObject["id"]))
            {
                person.Id = Guid.Parse( (string)personObject["id"]);
            }

            //var person = JsonConvert.DeserializeObject<Person>(personJson);
            log.LogInformation("Save a person");
            
            if (person.Id==Guid.Empty) PersonManager.CreatePerson(person);
            else PersonManager.UpdatePerson(person);

            return new OkObjectResult(true);
        }
    }
}
