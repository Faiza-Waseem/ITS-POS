using ITS_POS;
using ITS_POS.Data;
using ITS_POS.Entities;
using ITS_POS.Services;

using DataContextDb context = new DataContextDb();

UserAuthenticationService u = new UserAuthenticationService(context);
ProductManagementService p = new ProductManagementService(context);
InventoryManagementService i = new InventoryManagementService(context);
SalesTransactionService s = new SalesTransactionService(context);

//User userAdmin = new User() { Username = "Faiza", Password = "abc123", Email = "faiza@gmail.com", Role = "Admin"};
//User userCashier = new User() { Username = "Waseem", Password = "def456", Email = "waseem@gmail.com", Role = "Cashier" };

//u.RegisterUser(userAdmin);
//u.RegisterUser(userCashier);
////DataContext.DisplayUsers();

////Console.WriteLine(userAdmin);
////Console.WriteLine(userCashier);

//u.Login("Faiza", "abc123");
////u.Login("Waseem", "def456");

////u.SetUserRole("Waseem", "Admin");
////Console.WriteLine(userCashier);

//Product product1 = new Product() { ProductName = "Xiaomi", ProductType = "Mobile Phone", ProductCategory = "Electronics", ProductQuantity = 10, ProductPrice = 100000 };
////Console.WriteLine(product1);

//Product product2 = new Product() { ProductName = "IPhone", ProductType = "Mobile Phone", ProductCategory = "Electronics", ProductQuantity = 10, ProductPrice = 200000 };
//Product product3 = new Product() { ProductName = "Samsung", ProductType = "Tablet", ProductCategory = "Electronics", ProductQuantity = 20, ProductPrice = 300000 };
//Product product4 = new Product() { ProductName = "Lenovo", ProductType = "Laptop", ProductCategory = "Electronics", ProductQuantity = 30, ProductPrice = 400000 };
//Product product5 = new Product() { ProductName = "HP", ProductType = "Laptop", ProductCategory = "Electronics", ProductQuantity = 40, ProductPrice = 500000 };

//p.AddProductToInventory(product1);
////ProductManagement.AddProductToInventory(product1);

//p.AddProductToInventory(product2);
//p.AddProductToInventory(product3);
//p.AddProductToInventory(product4);
//p.AddProductToInventory(product5);

////DataContext.DisplayInventory();

////p.RemoveProductFromInventory("Xiaomi");
//p.UpdateProductInInventory("Xiaomi", "", "", 13, 0m);
////p.UpdateProductInInventory("Samsung", "abc", "def", 12, 1211213);

//Console.WriteLine(p.ViewProductFromInventory("Xiaomi"));

////DataContext.DisplayInventory();

//i.TrackProductQuantity("Lenovo");
//i.IncreaseProductQuantity("Lenovo", 25);
//i.TrackProductQuantity("Lenovo");

//i.CheckProductPrice("Lenovo");
//i.SetProductPrice("Lenovo", 450000);
//i.CheckProductPrice("Lenovo");

//u.Logout();
//u.Login("Waseem", "def456");

//s.AddProductToSale("Lenovo", 2);
////i.TrackProductQuantity("Lenovo");
//s.AddProductToSale("Xiaomi", 3);
//s.AddProductToSale("Samsung", 1);

//Console.WriteLine(s.CalculateAmountForSale());

//Console.WriteLine(s.GenerateReceipt());

//s.TransactSale();
//s.AddProductToSale("Xiaomi", 3);
//s.AddProductToSale("Samsung", 1);


//Console.WriteLine(s.GenerateReceipt());

////var products = context.Inventory.ToList();

////foreach (var product in products)
////{
////    Console.WriteLine(product);
////}

//u.Logout();
