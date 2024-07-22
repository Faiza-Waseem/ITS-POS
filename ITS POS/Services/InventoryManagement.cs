using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ITS_POS.Entities;
using ITS_POS.Data;

namespace ITS_POS.Services
{
    public static class InventoryManagement
    {
        public static void TrackProductQuantity(string productName)
        {
            if (UserAuthentication.CurrentUser != null)
            {
                var productInInventory = DataContext.Inventory.SingleOrDefault(p => p.ProductName == productName);

                if (productInInventory != null)
                {
                    Console.WriteLine($"Product Quantity is: {productInInventory.ProductQuantity}.");
                }
                else
                {
                    Console.WriteLine("Product Not Found.");
                }
            }
            else
            {
                Console.WriteLine("You are not currently logged in.");
            }
        }

        public static void CheckProductPrice(string productName)
        {
            if (UserAuthentication.CurrentUser != null)
            {
                var productInInventory = DataContext.Inventory.SingleOrDefault(p => p.ProductName == productName);

                if (productInInventory != null)
                {
                    Console.WriteLine($"Product Price is: {productInInventory.ProductPrice}.");
                }
                else
                {
                    Console.WriteLine("Product Not Found.");
                }
            }
            else
            {
                Console.WriteLine("You are not currently logged in.");
            }
        }

        public static void SetProductPrice(string productName, decimal newPrice)
        {
            if (UserAuthentication.CurrentUser != null)
            {
                if (UserAuthentication.CurrentUser.Role == "Admin")
                {
                    var productInInventory = DataContext.Inventory.SingleOrDefault(p => p.ProductName == productName);

                    if (productInInventory != null)
                    {
                        productInInventory.ProductPrice = newPrice;

                        Console.WriteLine("Product Price is changed successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Product Not Found.");
                    }
                }
                else
                {
                    Console.WriteLine("You are a Cashier. You don't have access to change product price.");
                }
            }
            else
            {
                Console.WriteLine("You are not currently logged in.");
            }
        }

        public static void IncreaseProductQuantity(string productName, int newQuantity)
        {
            if (UserAuthentication.CurrentUser != null)
            {
                if (UserAuthentication.CurrentUser.Role == "Admin")
                {
                    var productInInventory = DataContext.Inventory.SingleOrDefault(p => p.ProductName == productName);

                    if (productInInventory != null)
                    {
                        productInInventory.ProductQuantity += newQuantity;

                        Console.WriteLine($"Product Quantity is increased successfully by {newQuantity} items.");
                    }
                    else
                    {
                        Console.WriteLine("Product Not Found.");
                    }
                }
                else
                {
                    Console.WriteLine("You are a Cashier. You don't have access to increase product quantity.");
                }
            }
            else
            {
                Console.WriteLine("You are not currently logged in.");
            }
        }
    }
}
