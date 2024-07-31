using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_ITS.SERVICE.SalesService
{
    public interface ISalesService
    {
        Task AddProductToSaleAsync(int id, int quantity);
        decimal CalculateAmountForSale();
        string GenerateReceipt();
        Task TransactSaleAsync();
    }
}
