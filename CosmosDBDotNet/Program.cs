using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace CosmosDBDotNet
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateItem().Wait();
        }

        private static async Task CreateItem()
        {
            var cosmosUrl = "https://thongcosmosdemo.documents.azure.com:443/";
            var cosmosKey = "YMgUaq7UD42GaZVfJcco8Ok4VOwfSzgo8OggCJ1MMjSeRPc8p8a02nia6PMANNG0gpb7Wom2CMyvv5rRP2tvRA==";
            var databaseName = "DemoDB";

            CosmosClient client = new CosmosClient(cosmosUrl, cosmosKey);
            Database database = await client.CreateDatabaseIfNotExistsAsync(databaseName);
            Container container = await database.CreateContainerIfNotExistsAsync("MyContainerName", "/partitionKeyPath", 400);
            dynamic testItem = new { id = Guid.NewGuid().ToString(), partitionKeyPath = "MyTestPkValue", details = "it's ..." };
            var response = await container.CreateItemAsync(testItem);
        }
    }
}
