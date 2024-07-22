using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ITS_POS.Entities;
using ITS_POS.Data;
using System.Security;
using Microsoft.EntityFrameworkCore;

namespace ITS_POS.Services
{
    public class SalesTransaction
    {
        private static DataContextDb __context = null;

        public static void Initialize(DataContextDb context)
        {
            __context = context;
        }

        private static Sale CurrentSale { get; set; } = new Sale();

        public static void AddProductToSale(String productName, int quantity)
        {
            if (UserAuthentication.CurrentUser != null)
            {
                if (UserAuthentication.CurrentUser.Role == "Cashier")
                {
                    //var productInInventory = DataContext.Inventory.FirstOrDefault(p => p.ProductName == productName);
                    var productInInventory = __context.Inventory.FirstOrDefault(p => p.ProductName == productName);

                    if (productInInventory != null)
                    {
                        if (productInInventory.ProductQuantity < quantity)
                        {
                            Console.WriteLine($"Inventory has only {productInInventory.ProductQuantity} items. Do you want to add them to sale? Yes/No:");

                            string change = Console.ReadLine();

                            if (change == "Yes")
                            {
                                quantity = productInInventory.ProductQuantity;
                            }
                            else
                            {
                                Console.WriteLine("Product removed...");
                                return;
                            }
                        }
                        productInInventory.ProductQuantity -= quantity;

                        CurrentSale.SaleProducts.Add(new SaleProduct { Sale = CurrentSale, Product = productInInventory, Quantity = quantity});

                        Console.WriteLine("Product Added to Current Sale.");
                    }
                    else
                    {
                        Console.WriteLine("Product Not Found.");
                    }
                }
                else
                {
                    Console.WriteLine("You are an Admin. You don't have access to add a product to the sale.");
                }
            }
            else
            {
                Console.WriteLine("You are not currently logged in.");
            }
        }

        public static decimal CalculateAmountForSale()
        {
            decimal amount = 0;

            if (UserAuthentication.CurrentUser != null)
            {
                if (UserAuthentication.CurrentUser.Role == "Cashier")
                {
                    if (CurrentSale.SaleProducts.Count != 0)
                    {
                        foreach (var product in CurrentSale.SaleProducts)
                        {
                            amount += (product.Product.ProductPrice * product.Quantity);
                        }
                    }
                    else
                    {
                        Console.WriteLine("There is no product in current sale.");
                    }
                }
                else
                {
                    Console.WriteLine("You are an admin. You don't have access to calculate amount for current sale.");
                }
            }
            else 
            {
                Console.WriteLine("You are not currently logged in.");
            }

            return amount;
        }

        public static void GenerateReceipt()
        {
            if (UserAuthentication.CurrentUser != null)
            {
                if (UserAuthentication.CurrentUser.Role == "Cashier")
                {
                    if (CurrentSale.SaleProducts.Count != 0)
                    {
                        Console.WriteLine("Product Id\t\tProduct Name\t\tQuantity\t\tPrice\t\tTotal Price");

                        foreach (var product in CurrentSale.SaleProducts)
                        {
                            decimal total = product.Product.ProductPrice * (decimal)product.Quantity;
                            Console.WriteLine($"{product.Product.ProductId}\t\t\t{product.Product.ProductName}\t\t\t{product.Quantity}\t\t\t{product.Product.ProductPrice}\t\t{total}");
                        }

                        Console.WriteLine($"\n\nTotal Amount to be paid: {CalculateAmountForSale()}");
                    }
                    else
                    {
                        Console.WriteLine("There is no product in current sale.");
                    }
                }
                else
                {
                    Console.WriteLine("You are an admin. You don't have access to generate receipt for current sales.");
                }
            }
            else
            {
                Console.WriteLine("You are not currently logged in.");
            }
        }

        public static void TransactSale()
        {
            if (UserAuthentication.CurrentUser != null)
            {
                if (UserAuthentication.CurrentUser.Role == "Cashier")
                {
                    if (CurrentSale.SaleProducts.Count != 0)
                    {
                        //DataContext.Sales.Add(CurrentSale);

                        __context.Sales.Add(CurrentSale);
                        __context.SaveChanges();

                        CurrentSale.SaleProducts.Clear();
                    }
                    else
                    {
                        Console.WriteLine("There is no product in current sale.");
                    }
                }
                else
                {
                    Console.WriteLine("You are an Admin. You don't have access to make a sales transaction.");
                }
            }
            else
            {
                Console.WriteLine("You are not currently logged in.");
            }
        }
    }
}
