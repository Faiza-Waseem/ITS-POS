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
        #region Data Members

        private static DataContextDb __context = null;

        //Properties
        private static Sale CurrentSale { get; set; } = new Sale();

        #endregion

        #region Functions

        #region Get Context

        public static DataContextDb GetContext()
        {
            return __context;
        }

        #endregion

        #region Initialize

        public static void Initialize(DataContextDb context)
        {
            __context = context;
        }

        #endregion

        #region Product Addition to Sale

        public static void AddProductToSale(string productName, int quantity, out bool api)
        {
            if (UserAuthentication.CurrentUser == null)
            {
                Console.WriteLine("You are not currently logged in.");
                api = false;
                return;
            }

            if (UserAuthentication.CurrentUser.Role != "Cashier")
            {
                Console.WriteLine("You are an Admin. You don't have access to add a product to the sale.");
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
            
            if (product.ProductQuantity < quantity)
            {
                Console.WriteLine($"Inventory has only {product.ProductQuantity} items. Do you want to add them to sale? Yes/No:");

                string change = Console.ReadLine();

                if (change == "Yes")
                {
                    quantity = product.ProductQuantity;
                }
                else
                {
                    Console.WriteLine("Product removed...");
                    api = false;
                    return;
                }
            }

            product.ProductQuantity -= quantity;
            //var sp = CurrentSale.SaleProducts.SingleOrDefault(sp => sp.Product.ProductName == product.ProductName);
            var sp = CurrentSale.SaleProducts.SingleOrDefault(sp => sp.Product.ProductId == product.ProductId);
            
            if (sp == null)
            { 
                CurrentSale.SaleProducts.Add(new SaleProduct { Sale = CurrentSale, Product = product, Quantity = quantity }); 
            }
            else
            {
                sp.Quantity += quantity;
            }
            
            Console.WriteLine("Product Added to Current Sale.");
            api = true;
        }

        public static void AddProductToSale(String productName, int quantity)
        {
            bool api = false;
            AddProductToSale(productName, quantity, out api);
        }

        #endregion

        #region Sale Transaction

        public static decimal CalculateAmountForSale()
        {
            decimal amount = -1;

            if (UserAuthentication.CurrentUser == null)
            {
                Console.WriteLine("You are not currently logged in.");
            }
            else if (UserAuthentication.CurrentUser.Role != "Cashier")
            {
                Console.WriteLine("You are an admin. You don't have access to calculate amount for current sale.");
            }
            else if (CurrentSale.SaleProducts.Count == 0)
            {
                Console.WriteLine("There is no product in current sale.");
            }
            else
            {
                amount = 0;

                foreach (var product in CurrentSale.SaleProducts)
                {
                    amount += (product.Product.ProductPrice * product.Quantity);
                }
            }

            return amount;
        }

        public static string GenerateReceipt()
        {
            string receipt = "";

            if (UserAuthentication.CurrentUser == null)
            {
                Console.WriteLine("You are not currently logged in.");
            }
            else if (UserAuthentication.CurrentUser.Role != "Cashier")
            {
                Console.WriteLine("You are an admin. You don't have access to generate receipt for current sales.");
            }
            else if (CurrentSale.SaleProducts.Count == 0)
            {
                Console.WriteLine("There is no product in current sale.");
            }
            else
            {
                receipt += "Product Id\t\tProduct Name\t\tQuantity\t\tPrice\t\tTotal Price\n";

                foreach (var product in CurrentSale.SaleProducts)
                {
                    decimal total = product.Product.ProductPrice * (decimal)product.Quantity;
                    receipt += $"{product.Product.ProductId}\t\t\t{product.Product.ProductName}\t\t\t{product.Quantity}\t\t\t{product.Product.ProductPrice}\t\t{total}\n";
                }

                receipt += $"\nTotal Amount to be paid: {CalculateAmountForSale()}\n";
            }

            return receipt;
        }

        public static void TransactSale(out bool api)
        {
            if (UserAuthentication.CurrentUser == null)
            {
                Console.WriteLine("You are not currently logged in.");
                api = false;
                return;
            }

            if (UserAuthentication.CurrentUser.Role != "Cashier")
            {
                Console.WriteLine("You are an Admin. You don't have access to make a sales transaction.");
                api = false;
                return;
            }
            
            if (CurrentSale.SaleProducts.Count == 0)
            {
                Console.WriteLine("There is no product in current sale.");
                api = false;
                return;
            }

            //DataContext.Sales.Add(CurrentSale);

            __context.Sales.Add(CurrentSale);
            __context.SaveChanges();

            CurrentSale.SaleProducts.Clear();
            Console.WriteLine("Current Sale Transaction done.");
            api = true;
        }

        public static void TransactSale()
        {
            var api = false;
            TransactSale(out api);
        }

        #endregion

        #endregion
    }
}
