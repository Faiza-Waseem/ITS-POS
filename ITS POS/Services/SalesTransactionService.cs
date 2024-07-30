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
    public class SalesTransactionService : ServiceBase, ISalesTransactionService
    {
        #region Data Members

        //Properties
        private static Sale CurrentSale { get; set; } = new Sale();

        #endregion

        #region Constructor

        public SalesTransactionService(DataContextDb context) : base(context) { }

        #endregion

        #region Functions

        #region Get Context

        public override DataContextDb GetContext()
        {
            return ServiceBase.__context;
        }

        #endregion

        #region Product Addition to Sale

        public bool AddProductToSale(string productName, int quantity)
        {
            bool success = false;

            if (UserAuthenticationService.CurrentUser == null)
            {
                Console.WriteLine("You are not currently logged in.");
            }
            else if (UserAuthenticationService.CurrentUser.Role != "Cashier")
            {
                Console.WriteLine("You are an Admin. You don't have access to add a product to the sale.");
            }
            else
            {
                //var product = DataContext.Inventory.FirstOrDefault(p => p.ProductName == productName);
                var product = __context.Inventory.SingleOrDefault(p => p.ProductName == productName);

                if (product == null)
                {
                    Console.WriteLine("Product Not Found.");
                }
                else
                {
                    if (product.ProductQuantity < quantity)
                    {
                        Console.WriteLine($"Inventory has only {product.ProductQuantity} items. Do you want to add them to sale? Yes/No:");

                        string change = Console.ReadLine();

                        if (change != "Yes")
                        {
                            Console.WriteLine("Product removed...");
                            return false;
                        }

                        quantity = product.ProductQuantity;
                    }

                    product.ProductQuantity -= quantity;
                    var sp = CurrentSale.SaleProducts.SingleOrDefault(sp => sp.Product.ProductId == product.ProductId);

                    if (sp == null)
                    {
                        CurrentSale.SaleProducts.Add(new SaleProduct { /*Sale = CurrentSale,*/ Product = product, Quantity = quantity });
                    }
                    else
                    {
                        sp.Quantity += quantity;
                    }
                    
                    __context.SaveChanges();

                    Console.WriteLine("Product Added to Current Sale.");
                    success = true;
                }
            }

            return success;
        }

        #endregion

        #region Sale Transaction

        public decimal CalculateAmountForSale()
        {
            decimal amount = -1;

            if (UserAuthenticationService.CurrentUser == null)
            {
                Console.WriteLine("You are not currently logged in.");
            }
            else if (UserAuthenticationService.CurrentUser.Role != "Cashier")
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

        public string GenerateReceipt()
        {
            string receipt = "";

            if (UserAuthenticationService.CurrentUser == null)
            {
                Console.WriteLine("You are not currently logged in.");
            }
            else if (UserAuthenticationService.CurrentUser.Role != "Cashier")
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

        public bool TransactSale()
        {
            bool success = false;

            if (UserAuthenticationService.CurrentUser == null)
            {
                Console.WriteLine("You are not currently logged in.");
            }
            else if (UserAuthenticationService.CurrentUser.Role != "Cashier")
            {
                Console.WriteLine("You are an Admin. You don't have access to make a sales transaction.");
            }
            else if (CurrentSale.SaleProducts.Count == 0)
            {
                Console.WriteLine("There is no product in current sale.");
            }
            else
            {
                //DataContext.Sales.Add(CurrentSale);

                __context.Sales.Add(CurrentSale);
                __context.SaveChanges();

                CurrentSale.SaleProducts.Clear();
                CurrentSale = new Sale();
                
                Console.WriteLine("Current Sale Transaction done.");
                success = true;
            }

            return success;
        }
      
        #endregion

        #endregion
    }
}
