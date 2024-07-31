using POS_ITS.DATA;
using POS_ITS.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_ITS.REPOSITORIES.InventoryRepository
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly DataDbContext _context;

        public InventoryRepository(DataDbContext context)
        {
            _context = context;
        }

        public async Task<int> TrackProductQuantityAsync(int id)
        {
            try
            {
                var product = await _context.Inventory.FindAsync(id);

                if (product == null)
                {
                    throw new Exception($"No product was found with id {id}");
                }

                return product.ProductQuantity;
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
                var product = await _context.Inventory.FindAsync(id);

                if (product == null)
                {
                    throw new Exception($"No product was found with id {id}");
                }

                product.ProductQuantity += quantity;

                await _context.SaveChangesAsync();
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
                var product = await _context.Inventory.FindAsync(id);

                if (product == null)
                {
                    throw new Exception($"No product was found with id {id}");
                }

                return product.ProductPrice;
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
                var product = await _context.Inventory.FindAsync(id);

                if (product == null)
                {
                    throw new Exception($"No product was found with id {id}");
                }

                product.ProductPrice = price;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Repository: Error changing product price to {price} with id {id}: {ex.Message}");
            }
        }
    }
}
