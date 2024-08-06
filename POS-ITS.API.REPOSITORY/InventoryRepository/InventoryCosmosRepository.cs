using Microsoft.Azure.Cosmos;
using POS_ITS.DATA;
using POS_ITS.MODEL.Entities;
using POS_ITS.REPOSITORIES.InventoryRepository;
using System;
using System.Collections.Generic;
using System.Configuration.Internal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_ITS.REPOSITORIES.InventoryRepository
{
    public class InventoryCosmosRepository : IInventoryRepository
    {
        private readonly Container _inventoryContainer;

        public InventoryCosmosRepository(CosmosClient client, string databaseName)
        {
            var inventoryContainerName = "Inventory";
            _inventoryContainer = client.GetContainer(databaseName, inventoryContainerName);
        }

        public async Task<int> TrackProductQuantityAsync(int id)
        {
            try
            {
                var productId = "product_" + id.ToString();
                var productResponse = await _inventoryContainer.ReadItemAsync<Product>(productId, new PartitionKey(id));
                return productResponse.Resource.ProductQuantity;
            }
            catch (CosmosException cosmosEx)
            {
                throw new Exception($"CosmosDB Error: {cosmosEx.StatusCode} - {cosmosEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Repository: Error tracking product quantity with id {id}: {ex.Message}");
            }
        }

        public async Task IncreaseProductQuantityAsync(int id, int quantity)
        {
            try
            {
                var productId = "product_" + id.ToString();
                var productResponse = await _inventoryContainer.ReadItemAsync<Product>(productId, new PartitionKey(id));
                var product = productResponse.Resource;

                if(product == null)
                {
                    throw new Exception($"No product was found with id {id}");
                }

                product.ProductQuantity += quantity;
                await _inventoryContainer.ReplaceItemAsync(product, product.Id, new PartitionKey(product.ProductId));
            }
            catch (CosmosException cosmosEx)
            {
                throw new Exception($"CosmosDB Error: {cosmosEx.StatusCode} - {cosmosEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Repository: Error increasing product quantity by {quantity} with {id}: {ex.Message}");
            }
        }

        public async Task<decimal> GetProductPriceAsync(int id)
        {
            try
            {
                var productId = "product_" + id.ToString();
                var productResponse = await _inventoryContainer.ReadItemAsync<Product>(productId, new PartitionKey(id));
                return productResponse.Resource.ProductPrice;
            }
            catch (CosmosException cosmosEx)
            {
                throw new Exception($"CosmosDB Error: {cosmosEx.StatusCode} - {cosmosEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Repository: Error getting product price with id {id}: {ex.Message}");
            }
        }

        public async Task ChangeProductPriceAsync(int id, decimal price)
        {
            try
            {
                var productId = "product_" + id.ToString();
                var productResponse = await _inventoryContainer.ReadItemAsync<Product>(productId, new PartitionKey(id));
                var product = productResponse.Resource;

                if (product == null)
                {
                    throw new Exception($"No product was found with id {id}");
                }

                product.ProductPrice = price;
                await _inventoryContainer.ReplaceItemAsync(product, product.Id, new PartitionKey(product.ProductId));
            }
            catch (CosmosException cosmosEx)
            {
                throw new Exception($"CosmosDB Error: {cosmosEx.StatusCode} - {cosmosEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Repository: Error changing product price to {price} with id {id}: {ex.Message}");
            }
        }
    }
}
