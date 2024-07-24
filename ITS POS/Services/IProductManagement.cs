using ITS_POS.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS_POS.Services
{
    public interface IProductManagement
    {
        #region Functions

        #region Product Addition To Inventory

        public void AddProductToInventory(Product newProduct, out bool api);
        public void AddProductToInventory(Product newProduct);
        public void AddProductToInventory(string name, string type, string category, int quantity, decimal price);

        #endregion

        #region Product Removal From Inventory
        public void RemoveProductFromInventory(string productName, out bool api);
        public void RemoveProductFromInventory(string productName);

        #endregion

        #region View Product From Inventory

        public void ViewProductFromInventory(string productName, out Product inventoryProduct);
        public void ViewProductFromInventory(string productName);

        #endregion

        #region Update Product In Inventory

        public void UpdateProductInInventory(string productName, string productType, string productCategory, int productQuantity, decimal productPrice, out bool api);
        public void UpdateProductInInventory(string productName, string productType, string productCategory, int productQuantity, decimal productPrice);

        #endregion

        #endregion
    }
}
