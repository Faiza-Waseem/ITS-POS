using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ITS_POS.Entities;
using ITS_POS.Data;

namespace ITS_POS.Services
{
    public class ProductManagementService : ServiceBase, IProductManagementService
    {
        #region Constructor

        public ProductManagementService(DataContextDb context) : base(context) { }

        #endregion

        #region Functions

        #region Get Context

        public override DataContextDb GetContext()
        {
            return ServiceBase.__context;
        }

        #endregion

        #region Product Addition To Inventory

        public bool AddProductToInventory(string name, string type, string category, int quantity, decimal price)
        {
            bool success = false;

            if (UserAuthenticationService.CurrentUser == null)
            {
                Console.WriteLine("You are not currently logged in.");
            }
            else if (UserAuthenticationService.CurrentUser.Role != "Admin")
            {
                Console.WriteLine("You are a Cashier. You don't have access to add a product to the inventory.");
            }
            else
            {
                Product newProduct = new Product() { ProductName = name, ProductType = type, ProductCategory = category, ProductQuantity = quantity, ProductPrice = price };

                //var product = DataContext.Inventory.SingleOrDefault(p => p.ProductName == newProduct.ProductName);
                var product = __context.Inventory.SingleOrDefault(p => p.ProductName == newProduct.ProductName);

                if (product == null)
                {
                    //DataContext.Inventory.Add(newProduct);
                    __context.Inventory.Add(newProduct);
                    __context.SaveChanges();

                    Console.WriteLine("Product Added to the Inventory.");
                    success = true;
                }
                else
                {
                    Console.WriteLine("Product already exists in inventory.");
                    Console.WriteLine("Do you want to change its quantity? Yes/No:");

                    string change = Console.ReadLine();

                    if (change == "Yes")
                    {
                        product.ProductQuantity += newProduct.ProductQuantity;
                        __context.SaveChanges();
                        Console.WriteLine("Product Added to the Inventory.");
                        success = true;
                    }
                }
            }

            return success;
        }

        #endregion

        #region Product Removal From Inventory
        public bool RemoveProductFromInventory(string productName)
        {
            bool success = false;

            if (UserAuthenticationService.CurrentUser == null)
            {
                Console.WriteLine("You are not currently logged in.");
            }
            else if (UserAuthenticationService.CurrentUser.Role != "Admin")
            {
                Console.WriteLine("You are a Cashier. You don't have access to remove a product from the inventory.");
            }
            else
            {
                //var product = DataContext.Inventory.SingleOrDefault(p => p.ProductName == productName);
                var product = __context.Inventory.SingleOrDefault(p => p.ProductName == productName);

                if (product == null)
                {
                    Console.WriteLine("Product Not found.");
                }
                else
                {
                    //DataContext.Inventory.Remove(product);
                    __context.Inventory.Remove(product);
                    __context.SaveChanges();

                    Console.WriteLine("Product Removed from the Inventory.");
                    success = true;
                }
            }

            return success;
        }

        #endregion

        #region View Product From Inventory

        public string ViewProductFromInventory(string productName)
        {
            string inventoryProduct = "";

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
                    Console.WriteLine(product);
                    inventoryProduct = product.ToString();
                }
            }

            return inventoryProduct;
        }

        #endregion

        #region Update Product In Inventory

        public bool UpdateProductInInventory(string productName, string productType, string productCategory, int productQuantity, decimal productPrice)
        {
            bool success = false;

            if (UserAuthenticationService.CurrentUser == null)
            {
                Console.WriteLine("You are not currently logged in.");
            }
            else if (UserAuthenticationService.CurrentUser.Role != "Admin")
            {
                Console.WriteLine("You are a Cashier. You don't have access to update a product in the inventory.");
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
                    if (productType != "")
                    {
                        product.ProductType = productType;
                    }

                    if (productCategory != "")
                    {
                        product.ProductCategory = productCategory;
                    }

                    if (productQuantity != 0)
                    {
                        product.ProductQuantity = productQuantity;
                    }

                    if (productPrice != 0m)
                    {
                        product.ProductPrice = productPrice;
                    }

                    __context.SaveChanges();

                    Console.WriteLine("Product updated successfully.");
                    success = true;
                }
            }

            return success;
        }

        #endregion

        #region Get All Products

        public List<String> GetAllProducts()
        {
            List<string> products = new List<string>();
            var inventory = __context.Inventory.ToList().ToString();

            foreach ( var product in inventory )
            {
                products.Add(product.ToString());
            }

            return products;
        }

        #endregion

        #endregion
    }
}
