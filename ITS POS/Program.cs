using ITS_POS;
using ITS_POS.Data;
using ITS_POS.Entities;
using ITS_POS.Services;

using var context = new DataContextDb();

UserAuthentication.Initialize(context);
ProductManagement.Initialize(context);
InventoryManagement.Initialize(context);
SalesTransaction.Initialize(context);

User userAdmin = new User() { UserName = "Faiza", Password = "abc123", Email = "faiza@gmail.com", Role = "Admin"};
User userCashier = new User() { UserName = "Waseem", Password = "def456", Email = "waseem@gmail.com", Role = "Cashier" };

UserAuthentication.RegisterUser(userAdmin);
UserAuthentication.RegisterUser(userCashier);
//DataContext.DisplayUsers();

//Console.WriteLine(userAdmin);
//Console.WriteLine(userCashier);

UserAuthentication.Login("Faiza", "abc123");
//UserAuthentication.Login("Waseem", "def456");

//UserAuthentication.SetUserRole(userCashier, "Admin");
//Console.WriteLine(userCashier);

Product product1 = new Product() { ProductId = 1, ProductName = "Xiaomi", ProductType = "Mobile Phone", ProductCategory = "Electronics", ProductQuantity = 10, ProductPrice = 100000 };
//Console.WriteLine(product1);

Product product2 = new Product() { ProductId = 2, ProductName = "IPhone", ProductType = "Mobile Phone", ProductCategory = "Electronics", ProductQuantity = 10, ProductPrice = 200000 };
Product product3 = new Product() { ProductId = 3, ProductName = "Samsung", ProductType = "Tablet", ProductCategory = "Electronics", ProductQuantity = 20, ProductPrice = 300000 };
Product product4 = new Product() { ProductId = 4, ProductName = "Lenovo", ProductType = "Laptop", ProductCategory = "Electronics", ProductQuantity = 30, ProductPrice = 400000 };
Product product5 = new Product() { ProductId = 5, ProductName = "HP", ProductType = "Laptop", ProductCategory = "Electronics", ProductQuantity = 40, ProductPrice = 500000 };

ProductManagement.AddProductToInventory(product1);
//ProductManagement.AddProductToInventory(product1);

ProductManagement.AddProductToInventory(product2);
ProductManagement.AddProductToInventory(product3);
ProductManagement.AddProductToInventory(product4);
ProductManagement.AddProductToInventory(product5);

//DataContext.DisplayInventory();

//ProductManagement.RemoveProductFromInventory(product1);
//ProductManagement.UpdateProductInInventory("Xiaomi", "abc", "def", 12, 1211213);
//ProductManagement.UpdateProductInInventory("Samsung", "abc", "def", 12, 1211213);

//ProductManagement.ViewProductFromInventory("Lenovo");

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

SalesTransaction.GenerateReceipt();

SalesTransaction.TransactSale();
SalesTransaction.GenerateReceipt();

UserAuthentication.Logout();
