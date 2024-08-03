using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using POS_ITS.MODEL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_ITS.REPOSITORIES.ProductRepository
{
    public class ProductCosmosRepository : IProductRepository
    {
        private readonly IConfiguration _configuration;
        private readonly Container _productContainer;

        public ProductCosmosRepository(CosmosClient client, IConfiguration configuration)
        {
            _configuration = configuration;
            var databaseName = configuration["CosmosDbSettings:DatabaseName"];
            var productContainerName = "Inventory";
            _productContainer = client.GetContainer(databaseName, productContainerName);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            try
            {
                var query = new QueryDefinition("SELECT * FROM c");
                var products = new List<Product>();

                using (var feedIterator = _productContainer.GetItemQueryIterator<Product>(query))
                {
                    while (feedIterator.HasMoreResults)
                    {
                        var response = await feedIterator.ReadNextAsync();
                        products.AddRange(response);
                    }
                }

                return products;
            }
            catch (CosmosException cosmosEx)
            {
                throw new Exception($"CosmosDB Error: {cosmosEx.StatusCode} - {cosmosEx.Message}");
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                throw new Exception($"Repository: Error getting all products: {ex.Message}");
            }
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            try
            {
                var productId = "product_" + id.ToString();
                var product = await _productContainer.ReadItemAsync<Product>(productId, new PartitionKey(id));
                return product.Resource;
            }
            catch (CosmosException cosmosEx)
            {
                throw new Exception($"CosmosDB Error: {cosmosEx.StatusCode} - {cosmosEx.Message}");
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                throw new Exception($"Repository: Error getting product by id {id}: {ex.Message}");
            }
        }

        public async Task AddProductAsync(Product product)
        {
            try
            {
                await _productContainer.CreateItemAsync(product, new PartitionKey(product.ProductId));
            }
            catch (CosmosException cosmosEx)
            {
                throw new Exception($"CosmosDB Error: {cosmosEx.StatusCode} - {cosmosEx.Message}");
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                throw new Exception($"Repository: Error adding product: {ex.Message}");
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            try
            {
                var productToBeUpdated = await GetProductByIdAsync(product.ProductId);

                if (productToBeUpdated != null)
                {
                    productToBeUpdated = product;
                }
                else
                {
                    throw new Exception("No product was found with given id.");
                }

                await _productContainer.ReplaceItemAsync(productToBeUpdated, productToBeUpdated.Id, new PartitionKey(productToBeUpdated.ProductId));
            }
            catch (CosmosException cosmosEx)
            {
                throw new Exception($"CosmosDB Error: {cosmosEx.StatusCode} - {cosmosEx.Message}");
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                throw new Exception($"Repository: Error updating product with id {product.ProductId}: {ex.Message}");
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            try
            {
                var productId = "product_" + id.ToString();
                await _productContainer.DeleteItemAsync<Product>(productId, new PartitionKey(id));
            }
            catch (CosmosException cosmosEx)
            {
                throw new Exception($"CosmosDB Error: {cosmosEx.StatusCode} - {cosmosEx.Message}");
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                throw new Exception($"Repository: Error deleting product with id {id}: {ex.Message}");
            }
        }
    }
}
