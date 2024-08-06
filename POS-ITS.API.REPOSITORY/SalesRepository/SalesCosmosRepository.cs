using Microsoft.Azure.Cosmos;
using POS_ITS.MODEL.Entities;

namespace POS_ITS.REPOSITORIES.SalesRepository
{
    public class SalesCosmosRepository : ISalesRepository
    {
        private readonly Container _salesContainer;
        private readonly Container _saleProductsContainer;
        private readonly Container _productContainer;

        private static int saleCount = 0;
        private static int saleProductCount = 0;
        private static Sale CurrentSale { get; set; } = new Sale();

        public SalesCosmosRepository(CosmosClient client, string databaseName)
        {
            var salesContainerName = "Sales";
            _salesContainer = client.GetContainer(databaseName, salesContainerName);
            var saleProductsContainerName = "SaleProducts";
            _saleProductsContainer = client.GetContainer(databaseName, saleProductsContainerName);
            var productContainerName = "Inventory";
            _productContainer = client.GetContainer(databaseName, productContainerName);

            GetLatestSaleId();
            GetLatestSaleProductId();
        }

        private void GetLatestSaleId()
        {
            var query = "SELECT VALUE MAX(c.SaleId) FROM c";
            var iterator = _salesContainer.GetItemQueryIterator<int>(query);

            var response = iterator.ReadNextAsync().GetAwaiter().GetResult();
            saleCount = response.First() + 1;
        }

        private void GetLatestSaleProductId()
        {
            var query = "SELECT VALUE MAX(c.SaleProductId) FROM c";
            var iterator = _saleProductsContainer.GetItemQueryIterator<int>(query);

            var response = iterator.ReadNextAsync().GetAwaiter().GetResult();

            if (saleProductCount < response.First() + 1)
            {
                saleProductCount = response.First() + 1;
            }
        }

        public async Task AddProductToSaleAsync(int id, int quantity)
        {
            try
            {
                var productId = "product_" + id.ToString();
                var productResponse = await _productContainer.ReadItemAsync<Product>(productId, new PartitionKey(id));
                var product = productResponse.Resource;

                if (product == null)
                {
                    throw new Exception($"No product was found with id {id}");
                }

                if (product.ProductQuantity < quantity)
                {
                    throw new Exception($"Not enough quantity available in the inventory for the product with id {id}.");
                }

                product.ProductQuantity -= quantity;

                var saleProduct = CurrentSale.SaleProducts.SingleOrDefault(sp => sp.ProductId == product.ProductId);

                if (saleProduct == null)
                {
                    CurrentSale.SaleProducts.Add(new SaleProduct { Id = "sale_product_" + saleProductCount.ToString(), SaleProductId = saleProductCount, SaleId = saleCount, ProductId = product.ProductId, Quantity = quantity });
                    saleProductCount += 1;
                }
                else
                {
                    saleProduct.Quantity += quantity;
                }

                await _productContainer.ReplaceItemAsync(product, product.Id, new PartitionKey(product.ProductId));
            }
            catch (CosmosException cosmosEx)
            {
                throw new Exception($"CosmosDB Error: {cosmosEx.StatusCode} - {cosmosEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Repository: Error adding product with id {id} to sale: {ex.Message}");
            }
        }

        public async Task<decimal> CalculateAmountForSaleAsync()
        {
            try
            {
                if (CurrentSale.SaleProducts.Count == 0)
                {
                    throw new Exception("No items in current sale.");
                }

                decimal amount = 0;

                foreach (var saleProduct in CurrentSale.SaleProducts)
                {
                    var productId = "product_" + saleProduct.ProductId.ToString();
                    var productResponse = await _productContainer.ReadItemAsync<Product>(productId, new PartitionKey(saleProduct.ProductId));
                    var product = productResponse.Resource;

                    amount += (product.ProductPrice * saleProduct.Quantity);
                }

                return amount;
            }
            catch (CosmosException cosmosEx)
            {
                throw new Exception($"CosmosDB Error: {cosmosEx.StatusCode} - {cosmosEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Repository: Error calculating amount for current sale: {ex.Message}");
            }
        }

        public async Task<string> GenerateReceiptAsync()
        {
            try
            {
                if (CurrentSale.SaleProducts.Count == 0)
                {
                    throw new Exception("No items in current sale.");
                }

                string receipt = "Product Id\t\tProduct Name\t\tQuantity\t\tPrice\t\tTotal Price\n";

                foreach (var saleProduct in CurrentSale.SaleProducts)
                {
                    var productId = "product_" + saleProduct.ProductId.ToString();
                    var productResponse = await _productContainer.ReadItemAsync<Product>(productId, new PartitionKey(saleProduct.ProductId));
                    var product = productResponse.Resource;

                    decimal total = product.ProductPrice * (decimal)saleProduct.Quantity;
                    receipt += $"{product.ProductId}\t\t\t{product.ProductName}\t\t\t{saleProduct.Quantity}\t\t\t{product.ProductPrice}\t\t{total}\n";
                }
                decimal amount = await CalculateAmountForSaleAsync();
                receipt += $"\nTotal Amount to be paid: {amount}\n";

                return receipt;
            }
            catch (CosmosException cosmosEx)
            {
                throw new Exception($"CosmosDB Error: {cosmosEx.StatusCode} - {cosmosEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Repository: Error generating receipt for current sale: {ex.Message}");
            }
        }

        public async Task TransactSaleAsync()
        {
            try
            {
                if (CurrentSale.SaleProducts.Count == 0)
                {
                    throw new Exception("No items in current sale.");
                }

                CurrentSale.Id = "sale_" + saleCount.ToString();
                CurrentSale.SaleId = saleCount;
                saleCount += 1;
                await _salesContainer.CreateItemAsync(CurrentSale, new PartitionKey(CurrentSale.SaleId));

                foreach (var saleProduct in CurrentSale.SaleProducts)
                {
                    await _saleProductsContainer.CreateItemAsync(saleProduct, new PartitionKey(saleProduct.SaleProductId));
                }

                CurrentSale = new Sale();
            }
            catch (CosmosException cosmosEx)
            {
                throw new Exception($"CosmosDB Error: {cosmosEx.StatusCode} - {cosmosEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Repository: Error transacting current sale: {ex.Message}");
            }
        }
    }
}
