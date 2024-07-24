using ITS_POS;
using ITS_POS.Data;
using ITS_POS.Entities;
using ITS_POS.Services;

using var context = new DataContextDb();

ServiceBase.Initialize(context);

User userAdmin = new User() { Username = "Faiza", Password = "abc123", Email = "faiza@gmail.com", Role = "Admin"};
User userCashier = new User() { Username = "Waseem", Password = "def456", Email = "waseem@gmail.com", Role = "Cashier" };

UserAuthentication.RegisterUser(userAdmin);
UserAuthentication.RegisterUser(userCashier);
//DataContext.DisplayUsers();

//Console.WriteLine(userAdmin);
//Console.WriteLine(userCashier);

UserAuthentication.Login("Faiza", "abc123");
//UserAuthentication.Login("Waseem", "def456");

//UserAuthentication.SetUserRole("Waseem", "Admin");
//Console.WriteLine(userCashier);

Product product1 = new Product() { ProductName = "Xiaomi", ProductType = "Mobile Phone", ProductCategory = "Electronics", ProductQuantity = 10, ProductPrice = 100000 };
//Console.WriteLine(product1);

Product product2 = new Product() { ProductName = "IPhone", ProductType = "Mobile Phone", ProductCategory = "Electronics", ProductQuantity = 10, ProductPrice = 200000 };
Product product3 = new Product() { ProductName = "Samsung", ProductType = "Tablet", ProductCategory = "Electronics", ProductQuantity = 20, ProductPrice = 300000 };
Product product4 = new Product() { ProductName = "Lenovo", ProductType = "Laptop", ProductCategory = "Electronics", ProductQuantity = 30, ProductPrice = 400000 };
Product product5 = new Product() { ProductName = "HP", ProductType = "Laptop", ProductCategory = "Electronics", ProductQuantity = 40, ProductPrice = 500000 };

ProductManagement.AddProductToInventory(product1);
//ProductManagement.AddProductToInventory(product1);

ProductManagement.AddProductToInventory(product2);
ProductManagement.AddProductToInventory(product3);
ProductManagement.AddProductToInventory(product4);
ProductManagement.AddProductToInventory(product5);

//DataContext.DisplayInventory();

//ProductManagement.RemoveProductFromInventory("Xiaomi");
ProductManagement.UpdateProductInInventory("Xiaomi", "", "", 13, 0m);
//ProductManagement.UpdateProductInInventory("Samsung", "abc", "def", 12, 1211213);

ProductManagement.ViewProductFromInventory("Xiaomi");

//DataContext.DisplayInventory();

InventoryManagement.TrackProductQuantity("Lenovo");
InventoryManagement.IncreaseProductQuantity("Lenovo", 25);
InventoryManagement.TrackProductQuantity("Lenovo");

InventoryManagement.CheckProductPrice("Lenovo");
InventoryManagement.SetProductPrice("Lenovo", 450000);
InventoryManagement.CheckProductPrice("Lenovo");

UserAuthentication.Logout();
UserAuthentication.Login("Waseem", "def456");

SalesTransaction.AddProductToSale("Lenovo", 2);
//InventoryManagement.TrackProductQuantity("Lenovo");
SalesTransaction.AddProductToSale("Xiaomi", 3);
SalesTransaction.AddProductToSale("Samsung", 1);

Console.WriteLine(SalesTransaction.CalculateAmountForSale());

Console.WriteLine(SalesTransaction.GenerateReceipt());

SalesTransaction.TransactSale();
SalesTransaction.AddProductToSale("Xiaomi", 3);
SalesTransaction.AddProductToSale("Samsung", 1);


Console.WriteLine(SalesTransaction.GenerateReceipt());

//var products = context.Inventory.ToList();

//foreach (var product in products)
//{
//    Console.WriteLine(product);
//}

UserAuthentication.Logout();
