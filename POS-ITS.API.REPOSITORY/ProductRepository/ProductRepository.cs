using Microsoft.EntityFrameworkCore;
using POS_ITS.MODEL.Entities;
using POS_ITS.DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_ITS.REPOSITORIES.ProductRepository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataDbContext _context;

        public ProductRepository(DataDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            try
            {
                return await _context.Inventory.ToListAsync();
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
                return await _context.Inventory.FindAsync(id);
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
                _context.Inventory.Add(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                throw new Exception($"Repository: Error adding product: { ex.Message }");
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            try
            {
                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
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
                var product = await _context.Inventory.FindAsync(id);
                if (product != null)
                {
                    _context.Inventory.Remove(product);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"Product with id {id} not found");
                }
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                throw new Exception($"Repository: Error deleting product with id {id}: {ex.Message}");
            }
        }
    }
}
