using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ITS_POS.Entities;
using ITS_POS.Data;

namespace ITS_POS.Services
{
    public class ProductManagement
    {
        #region Data Members

        private static DataContextDb __context = null;

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

        #region Product Addition To Inventory

        public static void AddProductToInventory(Product newProduct, out bool api)
        {
            if (UserAuthentication.CurrentUser == null)
            {
                Console.WriteLine("You are not currently logged in.");
                api = false;
                return;
            }

            if (UserAuthentication.CurrentUser.Role != "Admin")
            {
                Console.WriteLine("You are a Cashier. You don't have access to add a product to the inventory.");
                api = false;
                return;
            }
            
            //var productInInventory = DataContext.Inventory.SingleOrDefault(p => p.ProductName == product.ProductName);
            var productInInventory = __context.Inventory.SingleOrDefault(p => p.ProductName == newProduct.ProductName);
           
            if (productInInventory == null)
            {
                //DataContext.Inventory.Add(product);
                __context.Inventory.Add(newProduct);
                __context.SaveChanges();

                Console.WriteLine("Product Added to the Inventory.");
                api = true;
                return;
            }

            Console.WriteLine("Product already exists in inventory.");
            Console.WriteLine("Do you want to change its quantity? Yes/No:");

            string change = Console.ReadLine();

            if (change != "Yes")
            {
                api = false;
                return;
            }

            productInInventory.ProductQuantity += newProduct.ProductQuantity;

            Console.WriteLine("Product Added to the Inventory.");
            api = true;
        }

        public static void AddProductToInventory(Product newProduct)
        {
            var api = false;
            AddProductToInventory(newProduct, out api);
        }

        public static void AddProductToInventory(string name, string type, string category, int quantity, decimal price)
        {
            Product newProduct = new Product() { ProductName = name, ProductType = type, ProductCategory = category, ProductQuantity = quantity, ProductPrice = price };
            AddProductToInventory(newProduct);
        }

        #endregion

        #region Product Removal From Inventory
        public static void RemoveProductFromInventory(string productName, out bool api)
        {
            if (UserAuthentication.CurrentUser == null)
            {
                Console.WriteLine("You are not currently logged in.");
                api = false;
                return;
            }

            if (UserAuthentication.CurrentUser.Role != "Admin")
            {
                Console.WriteLine("You are a Cashier. You don't have access to remove a product from the inventory.");
                api = false;
                return;
            }

            //var productInInventory = DataContext.Inventory.SingleOrDefault(p => p.ProductName == productName);
            var product = __context.Inventory.SingleOrDefault(p => p.ProductName == productName);

            if (product == null)
            {
                Console.WriteLine("Product Not found.");
                api = false;
                return;
            }

            //DataContext.Inventory.Remove(product);
            __context.Inventory.Remove(product);
            __context.SaveChanges();

            Console.WriteLine("Product Removed from the Inventory.");
            api = true;
        }

        public static void RemoveProductFromInventory(string productName)
        {
            var api = false;
            RemoveProductFromInventory(productName, out api);
        }

        #endregion

        #region View Product From Inventory

        public static void ViewProductFromInventory(string productName, out Product inventoryProduct)
        {
            if (UserAuthentication.CurrentUser == null)
            {
                Console.WriteLine("You are not currently logged in.");
                inventoryProduct = null;
                return;
            }

            //var product = DataContext.Inventory.SingleOrDefault(p => p.ProductName == productName);
            var product = __context.Inventory.SingleOrDefault(p => p.ProductName == productName);

            if (product == null)
            {
                Console.WriteLine("Product Not Found.");
            }

            Console.WriteLine(product);
            inventoryProduct = product;
        }

        public static void ViewProductFromInventory(string productName)
        {
            Product product = null;
            ViewProductFromInventory (productName, out product);
        }

        #endregion

        #region Update Product In Inventory

        public static void UpdateProductInInventory(string productName, string productType, string productCategory, int productQuantity, decimal productPrice, out bool api)
        {
            if (UserAuthentication.CurrentUser == null)
            {
                Console.WriteLine("You are not currently logged in.");
                api = false;
                return;
            }

            if (UserAuthentication.CurrentUser.Role != "Admin")
            {
                Console.WriteLine("You are a Cashier. You don't have access to update a product in the inventory.");
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

            Console.WriteLine("Product updated successfully.");
            api = true;
        }

        public static void UpdateProductInInventory(string productName, string productType, string productCategory, int productQuantity, decimal productPrice)
        {
            var api = false;
            UpdateProductInInventory(productName, productType, productCategory, productQuantity, productPrice, out api);
        }

        #endregion

        #endregion
    }
}
