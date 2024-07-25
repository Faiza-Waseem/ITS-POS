using ITS_POS.Data;
using ITS_POS.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS_POS.Services
{
    public interface ISalesTransactionService
    {
        #region Functions

        #region Get Context

        public DataContextDb GetContext();

        #endregion
        
        #region Product Addition to Sale

        public bool AddProductToSale(string productName, int quantity);

        #endregion

        #region Sale Transaction

        public decimal CalculateAmountForSale();
        public string GenerateReceipt();
        public bool TransactSale();

        #endregion

        #endregion
    }
}
