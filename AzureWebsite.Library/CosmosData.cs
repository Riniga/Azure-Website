using System;

namespace AzureWebsite.Library
{
    public class CosmosData
    {
        public string EndpointUrl = Environment.GetEnvironmentVariable("EndpointUrl");
        public string PrimaryKey = Environment.GetEnvironmentVariable("PrimaryKey");
        public string databaseId = Environment.GetEnvironmentVariable("DatabaseId");
        public string menuContainerId = Environment.GetEnvironmentVariable("MenuContainerId");
        public string usersContainerId = Environment.GetEnvironmentVariable("UsersContainerId");
        public string loginsContainerId = Environment.GetEnvironmentVariable("LoginsContainerId");
    }
}
