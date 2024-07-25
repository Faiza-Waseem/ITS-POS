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
    public interface IProductManagementService
    {
        #region Functions

        #region Get Context

        public DataContextDb GetContext();

        #endregion

        #region Product Addition To Inventory

        public bool AddProductToInventory(string name, string type, string category, int quantity, decimal price);

        #endregion

        #region Product Removal From Inventory
        public bool RemoveProductFromInventory(string productName);

        #endregion

        #region View Product From Inventory

        public string ViewProductFromInventory(string productName);

        #endregion

        #region Update Product In Inventory

        public bool UpdateProductInInventory(string productName, string productType, string productCategory, int productQuantity, decimal productPrice);

        #endregion

        #region Get All Products

        public List<String> GetAllProducts();

        #endregion

        #endregion
    }
}
