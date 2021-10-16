using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebPush;


namespace AzureWebApp.Function
{
    public static class SubscriptionService
    {
        public static Subscription TheSubscription;

        [FunctionName("save-subscription")]
        public static async Task<IActionResult> Save([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("Save Subscription.");
            string subscriptionJson = await new StreamReader(req.Body).ReadToEndAsync();

            TheSubscription = JsonConvert.DeserializeObject<Subscription>(subscriptionJson);

            Console.WriteLine("Subscription:"+ TheSubscription.endpoint);

            return new OkObjectResult($"success");
        }

        [FunctionName("send-notification")]
        public static async Task<IActionResult> Send([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            string publicKey = "BKhSHyPZOZiEhBfIvnLRosMKpWeprqHWXK5r7Pv650HYlOkpbn16-ri4tJubVNDvO7zhWSytqQhsh3ngsuv348M";
            string privateKey = "WCtNHUsiQ2pGSRRl06mgV1L0NKduw55E3WhTXb0hiRw";

            var subscription = new PushSubscription(TheSubscription.endpoint, TheSubscription.keys.p256dh, TheSubscription.keys.auth);
            var vapidDetails = new VapidDetails("mailto:rickard@nisses-gagner.se", publicKey, privateKey);

            var webPushClient = new WebPushClient();

            webPushClient.SendNotification(subscription, "Tja!", vapidDetails);
            log.LogInformation("Send Notification.");
            Console.WriteLine("We sent notification to subscription: ");
            
            return new OkObjectResult($"success");
        }
    }

    public class Subscription
    {
        public string endpoint;
        public DateTime? expirationTime;
        public Keys keys;
        

        public class Keys
        {
            public string p256dh;
            public string auth;
        }
    }

}
