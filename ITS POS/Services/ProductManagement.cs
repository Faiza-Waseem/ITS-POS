using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ITS_POS.Entities;
using ITS_POS.Data;

namespace ITS_POS.Services
{
    public class ProductManagement : ServiceBase, IProductManagement
    {
        #region Constructor

        public ProductManagement(DataContextDb context) : base(context) { }

        #endregion

        #region Functions

        #region Product Addition To Inventory

        public void AddProductToInventory(Product newProduct, out bool api)
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
            
            //var product = DataContext.Inventory.SingleOrDefault(p => p.ProductName == newProduct.ProductName);
            var product = __context.Inventory.SingleOrDefault(p => p.ProductName == newProduct.ProductName);
           
            if (product == null)
            {
                //DataContext.Inventory.Add(newProduct);
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

            product.ProductQuantity += newProduct.ProductQuantity;

            Console.WriteLine("Product Added to the Inventory.");
            api = true;
        }

        public void AddProductToInventory(Product newProduct)
        {
            var api = false;
            AddProductToInventory(newProduct, out api);
        }

        public void AddProductToInventory(string name, string type, string category, int quantity, decimal price)
        {
            Product newProduct = new Product() { ProductName = name, ProductType = type, ProductCategory = category, ProductQuantity = quantity, ProductPrice = price };
            AddProductToInventory(newProduct);
        }

        #endregion

        #region Product Removal From Inventory
        public void RemoveProductFromInventory(string productName, out bool api)
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

            //var product = DataContext.Inventory.SingleOrDefault(p => p.ProductName == productName);
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

        public void RemoveProductFromInventory(string productName)
        {
            var api = false;
            RemoveProductFromInventory(productName, out api);
        }

        #endregion

        #region View Product From Inventory

        public void ViewProductFromInventory(string productName, out Product inventoryProduct)
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

        public void ViewProductFromInventory(string productName)
        {
            Product product = null;
            ViewProductFromInventory (productName, out product);
        }

        #endregion

        #region Update Product In Inventory

        public void UpdateProductInInventory(string productName, string productType, string productCategory, int productQuantity, decimal productPrice, out bool api)
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

        public void UpdateProductInInventory(string productName, string productType, string productCategory, int productQuantity, decimal productPrice)
        {
            var api = false;
            UpdateProductInInventory(productName, productType, productCategory, productQuantity, productPrice, out api);
        }

        #endregion

        #endregion
    }
}
