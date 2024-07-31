using POS_ITS.REPOSITORIES.SalesRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_ITS.SERVICE.SalesService
{
    public class SalesService : ISalesService
    {
        private readonly ISalesRepository _repository;

        public SalesService(ISalesRepository repository)
        {
            _repository = repository;
        }

        public async Task AddProductToSaleAsync(int id, int quantity)
        {
            try
            {
                await _repository.AddProductToSaleAsync(id, quantity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Service: Error adding product with id {id} to current sale: {ex.Message}");
            }
        }

        public decimal CalculateAmountForSale()
        {
            try
            {
                return _repository.CalculateAmountForSale();
            }
            catch (Exception ex)
            {
                throw new Exception($"Service: Error calculating amount for current sale: {ex.Message}");
            }
        }

        public string GenerateReceipt()
        {
            try
            {
                return _repository.GenerateReceipt();
            }
            catch (Exception ex)
            {
                throw new Exception($"Service: Error generating receipt for current sale: {ex.Message}");
            }
        }
        public async Task TransactSaleAsync()
        {
            try
            {
                await _repository.TransactSaleAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Service: Error transacting current sale: {ex.Message}");
            }
        }
    }
}
