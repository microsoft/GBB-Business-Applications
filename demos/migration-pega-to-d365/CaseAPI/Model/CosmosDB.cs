using System;
using System.Net;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using CaseAPI.PegaClasses;
using Microsoft.AspNetCore.Http;

namespace CaseAPI.Model
{
    public class CosmosDB
    {
        /*COSMOS DB COnfig*/
        private static string EndpointUri = Environment.GetEnvironmentVariable("CosmosEndpointURI");
        // The primary key for the Azure Cosmos account.
        private static string PrimaryKey = Environment.GetEnvironmentVariable("CosmosPrimaryKey");

        // The Cosmos client instance
        private CosmosClient cosmosClient;

        // The database we will create
        private Database database;

        // The container we will create.
        private Container container;

        // The name of the database and container we will create
        private string databaseId = "pegacases";
        private string containerId = "autoloans";

        public async Task CreateOrUpdateItemAsync(Case item)
        {
            CosmosClient cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            database = cosmosClient.GetDatabase(databaseId);
            container = database.GetContainer(containerId);

            try
            {
                ItemResponse<Case> itemResponse = await container.ReadItemAsync<Case>(item.id, new PartitionKey(item.partitionId));

                ItemResponse<Case> updatedItemResponse = await container.ReplaceItemAsync<Case>(item,item.id, new PartitionKey(item.partitionId));
                
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                
                
                ItemResponse<Case> itemResponse = await container.CreateItemAsync<Case>(item, new PartitionKey(item.partitionId));
            }

            
        }
    }
}
