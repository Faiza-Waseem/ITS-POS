using Microsoft.EntityFrameworkCore;
using POS_ITS.MODEL.Entities;
using POS_ITS.DATA;
using POS_ITS.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_ITS.REPOSITORIES.SalesRepository
{
    public class SalesRepository : ISalesRepository
    {
        private readonly DataDbContext _context;
        private static Sale CurrentSale { get; set; } = new Sale();

        public SalesRepository(DataDbContext context)
        {
            _context = context;
        }

        // Property to expose CurrentSale for testing
        public Sale GetCurrentSaleForTesting => CurrentSale;

        public async Task AddProductToSaleAsync(int id, int quantity)
        {
            try
            {
                var product = await _context.Inventory.FindAsync(id);

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
                    CurrentSale.SaleProducts.Add(new SaleProduct { SaleId = CurrentSale.SaleId, ProductId = product.ProductId, Quantity = quantity });
                }
                else
                {
                    saleProduct.Quantity += quantity;
                }
                await _context.SaveChangesAsync();
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
                    var product = await _context.Inventory.SingleOrDefaultAsync(p => p.ProductId == saleProduct.ProductId);
                    amount += (product.ProductPrice * saleProduct.Quantity);
                }

                return amount;
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
                    var product = await _context.Inventory.SingleOrDefaultAsync(p => p.ProductId == saleProduct.ProductId);
                    decimal total = product.ProductPrice * (decimal)saleProduct.Quantity;
                    receipt += $"{product.ProductId}\t\t\t{product.ProductName}\t\t\t{saleProduct.Quantity}\t\t\t{product.ProductPrice}\t\t{total}\n";
                }
                var amount = await CalculateAmountForSaleAsync();
                receipt += $"\nTotal Amount to be paid: {amount}\n";

                return receipt;
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

                foreach (var product in _context.SalesProducts)
                {
                    Console.WriteLine(product.ProductId + product.SaleId);
                }

                CurrentSale = new Sale();
            }
            catch (Exception ex)
            {
                throw new Exception($"Repository: Error transacting current sale: {ex.Message}");
            }
        }
    }
}
