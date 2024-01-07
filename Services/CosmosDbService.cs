using Microsoft.Azure.Cosmos;
using AzureCosmosCRUDWebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;

namespace AzureCosmosCRUDWebAPI.Services
{
    public class CosmosDbService
    {
        private Container _container;

        public CosmosDbService(CosmosClient dbClient, string databaseName, string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddItemAsync(Family item)
        {
            try
            {
                await this._container.CreateItemAsync(item, new PartitionKey(item.PartitionKey));
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Cosmos DB error in AddItemAsync. Status code: {ex.StatusCode}, Message: {ex.Message}, StackTrace: {ex.StackTrace}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddItemAsync: {ex.Message}, StackTrace: {ex.StackTrace}");
                throw;
            }
        }


        // Read an item by id
        public async Task<Family> GetItemAsync(string id, string partitionKeyValue)
        {
            try
            {
                ItemResponse<Family> response = await this._container.ReadItemAsync<Family>(id, new PartitionKey(partitionKeyValue));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        // Update an existing item
        public async Task UpdateItemAsync(string id, Family item)
        {
            await this._container.UpsertItemAsync(item, new PartitionKey(id));
        }

        // Delete an item
        public async Task DeleteItemAsync(string id, string partitionKeyValue)
        {
            await this._container.DeleteItemAsync<Family>(id, new PartitionKey(partitionKeyValue));
        }

        // List all items
        public async Task<IEnumerable<Family>> GetItemsAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<Family>(new QueryDefinition(queryString));
            List<Family> results = new List<Family>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.ToList());
            }

            return results;
        }
    }
}
