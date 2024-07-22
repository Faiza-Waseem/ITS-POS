using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ITS_POS.Entities;
using ITS_POS.Data;

namespace ITS_POS.Services
{
    public static class ProductManagement
    {
        public static void AddProductToInventory(Product product)
        {
            if (UserAuthentication.CurrentUser != null)
            {
                if (UserAuthentication.CurrentUser.Role == "Admin")
                {
                    var productInInventory = DataContext.Inventory.SingleOrDefault(p => p.ProductName == product.ProductName);
                    if (productInInventory != null)
                    {
                        Console.WriteLine("Product already exists in inventory.");
                        Console.WriteLine("Do you want to change its quantity? Yes/No:");

                        string change = Console.ReadLine();

                        if (change == "Yes")
                        {
                            productInInventory.ProductQuantity += product.ProductQuantity;

                            Console.WriteLine("Product Added to the Inventory.");
                        }
                    }
                    else
                    {
                        DataContext.Inventory.Add(product);
                        Console.WriteLine("Product Added to the Inventory.");
                    }
                }
                else
                {
                    Console.WriteLine("You are a Cashier. You don't have access to add a product to the inventory.");
                }
            }
            else
            {
                Console.WriteLine("You are not currently logged in.");
            }
        }

        public static void RemoveProductFromInventory(Product product)
        {
            if (UserAuthentication.CurrentUser != null)
            {
                if (UserAuthentication.CurrentUser.Role == "Admin")
                {
                    var productInInventory = DataContext.Inventory.SingleOrDefault(p => p.ProductName == product.ProductName);

                    if (productInInventory != null)
                    {
                        DataContext.Inventory.Remove(productInInventory);

                        Console.WriteLine("Product Removed from the Inventory.");
                    }
                    else
                    {
                        Console.WriteLine("Product Not found.");
                    }
                }
                else
                {
                    Console.WriteLine("You are a Cashier. You don't have access to remove a product from the inventory.");
                }
            }
            else
            {
                Console.WriteLine("You are not currently logged in.");
            }
        }

        public static void ViewProductFromInventory(string productName)
        {
            if (UserAuthentication.CurrentUser != null)
            {
                var productInInventory = DataContext.Inventory.SingleOrDefault(p => p.ProductName == productName);

                if (productInInventory != null)
                {
                    Console.WriteLine(productInInventory);
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

        public static void UpdateProductInInventory(string productName, string productType, string productCategory, int productQuantity, decimal productPrice)
        {
            if (UserAuthentication.CurrentUser != null)
            {
                if (UserAuthentication.CurrentUser.Role == "Admin")
                {
                    var productInInventory = DataContext.Inventory.SingleOrDefault(p => p.ProductName == productName);
                    if (productInInventory != null)
                    {
                        productInInventory.ProductType = productType;
                        productInInventory.ProductCategory = productCategory;
                        productInInventory.ProductQuantity = productQuantity;
                        productInInventory.ProductPrice = productPrice;
                    }
                    else
                    {
                        Console.WriteLine("Product Not Found.");
                    }
                }
                else
                {
                    Console.WriteLine("You are a Cashier. You don't have access to update a product in the inventory.");
                }
            }
            else
            {
                Console.WriteLine("You are not currently logged in.");
            }
        }
    }
}
