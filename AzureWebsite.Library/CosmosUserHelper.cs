using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace AzureWebsite.Library
{
    public class CosmosUserHelper
    {
        private static string partitionKeyPath = "/username";

        private static CosmosData data = new CosmosData();

        public static async Task<User> GetUserFromCosmosDb(string username)
        {
            var queryDefinition = new QueryDefinition("SELECT * FROM Users WHERE Users.username='" + username+"'");
            var container = await GetContainer();
            var queryResultSetIterator = container.GetItemQueryIterator<User>(queryDefinition);
            List<User> users = new List<User>();

            while (queryResultSetIterator.HasMoreResults)
            {
                var currentResultSet = await queryResultSetIterator.ReadNextAsync();
                users.AddRange(currentResultSet);
            }
            return users.FirstOrDefault();
        }


        public static async Task CreateUserInCosmosDb(string username, string name, string passwordhash)
        {
            Console.WriteLine("Create user with username: " + username);
            var user = await GetUserFromCosmosDb(username);


            if (user != null)
            {
                Console.WriteLine("Delete user with username: " + user.Username);
                await DeleteUserFromCosmosDb(user.Username);
            }

            user = new User() { Username=username, Name=name, PasswordHash=passwordhash, Role=UserRole.User };
            var container = await GetContainer();
            await container.CreateItemAsync<User>(user, new PartitionKey(user.Username.ToString()));
        }


        internal static async Task DeleteUserFromCosmosDb(string username)
        {
            var container = await GetContainer();
            await container.DeleteItemAsync<User>(username, new PartitionKey(username));
        }

        private static async Task<Container> GetContainer()
        {
            var cosmosClient = new CosmosClient(data.EndpointUrl, data.PrimaryKey);
            Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(data.databaseId);
            Container container = await database.CreateContainerIfNotExistsAsync(data.usersContainerId, partitionKeyPath);
            return container;
        }
    }
}
