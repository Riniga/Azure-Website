using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace AzureWebsite.Library
{
    public class CosmosHelper
    {
        private static string partitionKeyPath = "/id";
        private static CosmosData data = new CosmosData();

        public static async Task<Menu> GetMenuFromCosmosDb()
        {
            var queryDefinition = new QueryDefinition("SELECT * FROM menu"); 
            var container = await GetContainer();
            var queryResultSetIterator = container.GetItemQueryIterator<Menu>(queryDefinition);
            List<Menu> menues= new List<Menu>();
            
            while (queryResultSetIterator.HasMoreResults)
            {
                var currentResultSet = await queryResultSetIterator.ReadNextAsync();
                menues.AddRange(currentResultSet);
            }
            return menues.FirstOrDefault();
        }

        public static async Task CreateMenuInCosmosDb()
        {
            var menu = await GetMenuFromCosmosDb();
            if (menu!=null) await DeleteMenuFromCosmosDb(menu.id.ToString());
            menu = new Menu();
            menu.GenerateDummy();
            var container = await GetContainer();
            await container.CreateItemAsync<Menu>(menu, new PartitionKey(menu.id.ToString()));
        }
        internal static async Task DeleteMenuFromCosmosDb(string id)
        {
            var container = await GetContainer();
            await container.DeleteItemAsync<Menu>(id, new PartitionKey(id));
        }
        internal static async Task UpdateMenuInCosmosDb(Menu menu)
        {
            var container = await GetContainer();
            await container.ReplaceItemAsync<Menu>(menu, menu.id.ToString(), new PartitionKey(menu.id.ToString()));
        }

        private static async Task<Container> GetContainer()
        {
            var cosmosClient = new CosmosClient(data.EndpointUrl, data.PrimaryKey);
            Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(data.databaseId);
            Container container = await database.CreateContainerIfNotExistsAsync(data.containerId, partitionKeyPath);
            return container;
        }
    }
}
