using POS_ITS.DATA;
using POS_ITS.REPOSITORIES.InventoryRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_ITS.SERVICE.InventoryService
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _repository;

        public InventoryService(IInventoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> TrackProductQuantityAsync(int id)
        {
            try
            {
                return await _repository.TrackProductQuantityAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Service: Error tracking product quantity with id {id}: {ex.Message}");
            }
        }

        public async Task IncreaseProductQuantityAsync(int id, int quantity)
        {
            try
            {
                await _repository.IncreaseProductQuantityAsync(id, quantity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Service: Error increasing product quantity by {quantity} with {id}: {ex.Message}");
            }
        }

        public async Task<decimal> GetProductPriceAsync(int id)
        {
            try
            {
                return await _repository.GetProductPriceAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Service: Error getting product price with id {id}: {ex.Message}");
            }
        }

        public async Task ChangeProductPriceAsync(int id, decimal price)
        {
            try
            {
                await _repository.ChangeProductPriceAsync(id, price);
            }
            catch (Exception ex)
            {
                throw new Exception($"Service: Error changing product price to {price} with id {id}: {ex.Message}");
            }
        }
    }
}
