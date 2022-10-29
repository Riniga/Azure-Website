using System.Threading.Tasks;
using AzureWebsite.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using Newtonsoft.Json.Linq;
using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using AzureWebsite.Function;

namespace AzureWebApp.Function
{
    public static class Authenticator
    {
        static Authenticator()
        {
            FunctionsAssemblyResolver.RedirectAssembly();
        }

        [FunctionName("Login")]
        public static async Task<IActionResult> Login([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            string userJson= await new StreamReader(req.Body).ReadToEndAsync();
            var userObject = JObject.Parse(userJson);
            var login = await CosmosLoginsHelper.TryLoginUserInCosmosDb((string)userObject["username"], HashPassword((string)userObject["password"]));
            
            Console.WriteLine("Token: " + login.Token);

            return new OkObjectResult(login);
        }

        [FunctionName("Logout")]
        public static async Task<IActionResult> Logout([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            Console.WriteLine("Here");

            string userJson = await new StreamReader(req.Body).ReadToEndAsync();
            var userObject = JObject.Parse(userJson);
            
            Console.WriteLine("Logout user: " + (string)userObject["username"]) ;

            await CosmosLoginsHelper.Logout((string)userObject["username"], (string)userObject["token"]);
            return new OkObjectResult(true);
        }

        [FunctionName("CreateUser")]
        public static async Task<IActionResult> CreateUser([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            string userJson = await new StreamReader(req.Body).ReadToEndAsync();
            var userObject = JObject.Parse(userJson);
            await CosmosUserHelper.CreateUserInCosmosDb((string)userObject["username"], (string)userObject["name"], HashPassword((string)userObject["password"]));
            return new OkObjectResult(true);
        }

        private static string HashPassword(string password)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: Encoding.ASCII.GetBytes("AzureWebsite"),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            return hashed;
        }
    }
}
