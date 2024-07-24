using ITS_POS.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS_POS.Services
{
    public interface ISalesTransaction
    {
        #region Functions

        #region Product Addition to Sale

        public void AddProductToSale(string productName, int quantity, out bool api);
        public void AddProductToSale(String productName, int quantity);

        #endregion

        #region Sale Transaction

        public decimal CalculateAmountForSale();
        public string GenerateReceipt();
        public void TransactSale(out bool api);
        public void TransactSale();

        #endregion

        #endregion
    }
}
