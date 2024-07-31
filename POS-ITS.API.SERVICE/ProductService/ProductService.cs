using POS_ITS.MODEL.Entities;
using POS_ITS.REPOSITORIES.ProductRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_ITS.SERVICE.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            try
            {
                return await _repository.GetAllProductsAsync();
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                throw new Exception($"Service: Error getting all products: {ex.Message}");
            }
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            try
            {
                return await _repository.GetProductByIdAsync(id);
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                throw new Exception($"Service: Error getting product by id {id}: {ex.Message}");
            }
        }

        public async Task AddProductAsync(Product product)
        {
            try
            {
                await _repository.AddProductAsync(product);
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                throw new Exception($"Service: Error adding product: {ex.Message}");
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            try
            {
                await _repository.UpdateProductAsync(product);
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                throw new Exception($"Service: Error updating product with id {product.ProductId}: {ex.Message}");
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            try
            {
                await _repository.DeleteProductAsync(id);
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                throw new Exception($"Service: Error deleting product with id {id}: {ex.Message}");
            }
        }
    }
}
