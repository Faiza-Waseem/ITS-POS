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
    public interface IInventoryManagementService
    {
        #region Functions

        #region Get Context

        public DataContextDb GetContext();

        #endregion

        #region Product Tracking

        #region Product Quantity

        public int TrackProductQuantity(string productName);
        public bool IncreaseProductQuantity(string productName, int newQuantity);

        #endregion

        #region Product Price

        public decimal CheckProductPrice(string productName);
        public bool SetProductPrice(string productName, decimal newPrice);

        #endregion

        #endregion

        #region Get Inventory

        public List<string> GetInventory();

        #endregion

        #endregion
    }
}
