using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ITS_POS.Entities;
using ITS_POS.Data;

namespace ITS_POS.Services
{
    public class InventoryManagement : ServiceBase, IInventoryManagement
    {
        #region Constructor

        public InventoryManagement(DataContextDb context) : base(context) { }

        #endregion

        #region Functions

        #region Product Tracking

        #region Product Quantity
        public void TrackProductQuantity(string productName, out int quantity)
        {
            if (UserAuthentication.CurrentUser == null)
            {
                Console.WriteLine("You are not currently logged in.");
                quantity = -1;
                return;
            }

            //var product = DataContext.Inventory.SingleOrDefault(p => p.ProductName == productName);
            var product = __context.Inventory.SingleOrDefault(p => p.ProductName == productName);

            if (product == null)
            {
                Console.WriteLine("Product Not Found.");
                quantity = -1;
                return;
            }

            Console.WriteLine($"Product Quantity is: {product.ProductQuantity}.");
            quantity = product.ProductQuantity;
        }

        public void TrackProductQuantity(string productName)
        {
            int quantity = -1;
            TrackProductQuantity(productName, out quantity);
        }

        public void IncreaseProductQuantity(string productName, int newQuantity, out bool api)
        {
            if (UserAuthentication.CurrentUser == null)
            {
                Console.WriteLine("You are not currently logged in.");
                api = false;
                return;
            }

            if (UserAuthentication.CurrentUser.Role != "Admin")
            {
                Console.WriteLine("You are a Cashier. You don't have access to increase product quantity.");
                api = false;
                return;
            }

            //var product = DataContext.Inventory.SingleOrDefault(p => p.ProductName == productName);
            var product = __context.Inventory.SingleOrDefault(p => p.ProductName == productName);

            if (product == null)
            {
                Console.WriteLine("Product Not Found.");
                api = false;
                return;
            }

            product.ProductQuantity += newQuantity;
            Console.WriteLine($"Product Quantity is increased successfully by {newQuantity} items.");
            api = true;
        }

        public void IncreaseProductQuantity(string productName, int newQuantity)
        {
            var api = false;
            IncreaseProductQuantity(productName, newQuantity, out api);
        }

        #endregion

        #region Product Price

        public void CheckProductPrice(string productName, out decimal price)
        {
            if (UserAuthentication.CurrentUser == null)
            {
                Console.WriteLine("You are not currently logged in.");
                price = -1;
                return;
            }

            //var product = DataContext.Inventory.SingleOrDefault(p => p.ProductName == productName);
            var product = __context.Inventory.SingleOrDefault(p => p.ProductName == productName);

            if (product == null)
            {
                Console.WriteLine("Product Not Found.");
                price = -1;
                return;
            }

            Console.WriteLine($"Product Price is: {product.ProductPrice}.");
            price = product.ProductPrice;
        }

        public void CheckProductPrice(string productName)
        {
            decimal price = -1;
            CheckProductPrice(productName, out price);
        }
        
        public void SetProductPrice(string productName, decimal newPrice, out bool api)
        {
            if (UserAuthentication.CurrentUser == null)
            {
                Console.WriteLine("You are not currently logged in.");
                api = false;
                return;
            }

            if (UserAuthentication.CurrentUser.Role != "Admin")
            {
                Console.WriteLine("You are a Cashier. You don't have access to change product price.");
                api = false;
                return;
            }
            
            //var product = DataContext.Inventory.SingleOrDefault(p => p.ProductName == productName);
            var product = __context.Inventory.SingleOrDefault(p => p.ProductName == productName);

            if (product == null)
            {
                Console.WriteLine("Product Not Found.");
                api = false;
                return;
            }
            
            product.ProductPrice = newPrice;
            Console.WriteLine("Product Price is changed successfully.");
            api = true;
        }

        public void SetProductPrice(string productName, decimal newPrice)
        {
            var api = false;
            SetProductPrice(productName, newPrice, out api);
        }

        #endregion

        #endregion

        #endregion
    }
}
