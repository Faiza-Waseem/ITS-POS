using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ITS_POS.Entities;
using ITS_POS.Data;

namespace ITS_POS.Services
{
    public class InventoryManagementService : ServiceBase, IInventoryManagementService
    {
        #region Constructor

        public InventoryManagementService(DataContextDb context) : base(context) { }

        #endregion

        #region Functions

        #region Get Context

        public override DataContextDb GetContext()
        {
            return ServiceBase.__context;
        }

        #endregion

        #region Product Tracking

        #region Product Quantity
        public int TrackProductQuantity(string productName)
        {
            int quantity = -1;

            if (UserAuthenticationService.CurrentUser == null)
            {
                Console.WriteLine("You are not currently logged in.");
            }
            else
            {
                //var product = DataContext.Inventory.SingleOrDefault(p => p.ProductName == productName);
                var product = __context.Inventory.SingleOrDefault(p => p.ProductName == productName);

                if (product == null)
                {
                    Console.WriteLine("Product Not Found.");
                }
                else
                {
                    Console.WriteLine($"Product Quantity is: {product.ProductQuantity}.");
                    quantity = product.ProductQuantity;
                }
            }

            return quantity;
        }

        public bool IncreaseProductQuantity(string productName, int newQuantity)
        {
            bool success = false;

            if (UserAuthenticationService.CurrentUser == null)
            {
                Console.WriteLine("You are not currently logged in.");
            }
            else if (UserAuthenticationService.CurrentUser.Role != "Admin")
            {
                Console.WriteLine("You are a Cashier. You don't have access to increase product quantity.");
            }
            else
            {
                //var product = DataContext.Inventory.SingleOrDefault(p => p.ProductName == productName);
                var product = __context.Inventory.SingleOrDefault(p => p.ProductName == productName);

                if (product == null)
                {
                    Console.WriteLine("Product Not Found.");
                }
                else
                {
                    product.ProductQuantity += newQuantity;
                    __context.SaveChanges();
                    
                    Console.WriteLine($"Product Quantity is increased successfully by {newQuantity} items.");
                    success = true;
                }
            }

            return success;
        }

        #endregion

        #region Product Price

        public decimal CheckProductPrice(string productName)
        {
            decimal price = -1;

            if (UserAuthenticationService.CurrentUser == null)
            {
                Console.WriteLine("You are not currently logged in.");
            }
            else
            {
                //var product = DataContext.Inventory.SingleOrDefault(p => p.ProductName == productName);
                var product = __context.Inventory.SingleOrDefault(p => p.ProductName == productName);

                if (product == null)
                {
                    Console.WriteLine("Product Not Found.");
                    price = -1;
                }
                else
                {
                    Console.WriteLine($"Product Price is: {product.ProductPrice}.");
                    price = product.ProductPrice;
                }
            }

            return price;
        }

        public bool SetProductPrice(string productName, decimal newPrice)
        {
            bool success = false;

            if (UserAuthenticationService.CurrentUser == null)
            {
                Console.WriteLine("You are not currently logged in.");
            }
            else if (UserAuthenticationService.CurrentUser.Role != "Admin")
            {
                Console.WriteLine("You are a Cashier. You don't have access to change product price.");
            }
            else
            {
                //var product = DataContext.Inventory.SingleOrDefault(p => p.ProductName == productName);
                var product = __context.Inventory.SingleOrDefault(p => p.ProductName == productName);

                if (product == null)
                {
                    Console.WriteLine("Product Not Found.");
                }
                else
                {
                    product.ProductPrice = newPrice;
                    __context.SaveChanges();

                    Console.WriteLine("Product Price is changed successfully.");
                    success = true;
                }
            }

            return success;
        }

        #endregion

        #endregion

        #region Get Inventory

        public List<string> GetInventory()
        {
            List<string> products = new List<string>();
            var inventory = __context.Inventory.ToList().ToString();

            foreach (var product in inventory)
            {
                products.Add(product.ToString());
            }

            return products;
        }

        #endregion

        #endregion
    }
}
