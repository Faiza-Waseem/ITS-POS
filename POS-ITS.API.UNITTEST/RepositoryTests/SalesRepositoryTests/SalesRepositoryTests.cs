﻿using Microsoft.EntityFrameworkCore;
using POS_ITS.DATA;
using POS_ITS.MODEL.Entities;
using POS_ITS.REPOSITORIES.SalesRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_ITS.API.UNITTEST.RepositoryTests.SalesRepositoryTests
{
    public class SalesRepositoryTests
    {
        private DbContextOptions<DataDbContext> _dbContextOptions;
        private DataDbContext _context;
        private SalesRepository _repository;

        [SetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DataDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new DataDbContext(_dbContextOptions);
            _repository = new SalesRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context?.Dispose();
        }

        [Test]
        public async Task AddProductToSaleAsync_AddsProductToCurrentSale()
        {
            //Arrange
            var product = new Product
            {
                ProductId = 1,
                ProductName = "Product 1",
                ProductType = "Type 1",
                ProductCategory = "Category 1",
                ProductQuantity = 10,
                ProductPrice = 100
            };

            _context.Inventory.Add(product);
            await _context.SaveChangesAsync();

            //Act 
            await _repository.AddProductToSaleAsync(1, 3);

            //Assert
            var saleProduct = _repository.GetCurrentSaleForTesting.SaleProducts.First();
            var productQuantityCheck = await _context.Inventory.FindAsync(1);

            Assert.IsNotNull(saleProduct);
            Assert.That(saleProduct.Quantity, Is.EqualTo(3));
            Assert.IsNotNull(productQuantityCheck);
            Assert.That(productQuantityCheck.ProductQuantity, Is.EqualTo(7));
        }

        [Test]
        public async Task CalculateAmountForSaleAsync_ReturnsCurrentSalesAmount()
        {
            // Arrange
            var product1 = new Product
            {
                ProductId = 1,
                ProductName = "Product 1",
                ProductType = "Type 1",
                ProductCategory = "Category 1",
                ProductQuantity = 10,
                ProductPrice = 100
            };

            var product2 = new Product
            {
                ProductId = 2,
                ProductName = "Product 2",
                ProductType = "Type 2",
                ProductCategory = "Category 2",
                ProductQuantity = 20,
                ProductPrice = 200
            };

            _context.Inventory.AddRange(product1, product2);
            _context.SaveChanges();

            _repository.GetCurrentSaleForTesting.SaleProducts.Add(new SaleProduct { ProductId = 1, Quantity = 2 });
            _repository.GetCurrentSaleForTesting.SaleProducts.Add(new SaleProduct { ProductId = 2, Quantity = 1 });

            // Act
            var totalAmount = await _repository.CalculateAmountForSaleAsync();

            // Assert
            Assert.That(totalAmount, Is.EqualTo(400));
        }

        [Test]
        public async Task GenerateReceiptAsync_ReturnsCurrentSaleReceipt()
        {
            // Arrange
            var product1 = new Product
            {
                ProductId = 1,
                ProductName = "Product 1",
                ProductType = "Type 1",
                ProductCategory = "Category 1",
                ProductQuantity = 10,
                ProductPrice = 100
            };

            var product2 = new Product
            {
                ProductId = 2,
                ProductName = "Product 2",
                ProductType = "Type 2",
                ProductCategory = "Category 2",
                ProductQuantity = 5,
                ProductPrice = 200
            };

            _context.Inventory.AddRange(product1, product2);
            _context.SaveChanges();

            _repository.GetCurrentSaleForTesting.SaleProducts.Add(new SaleProduct { ProductId = 1, Quantity = 2 });
            _repository.GetCurrentSaleForTesting.SaleProducts.Add(new SaleProduct { ProductId = 2, Quantity = 1 });

            string expectedReceipt = "Product Id\t\tProduct Name\t\tQuantity\t\tPrice\t\tTotal Price\n" +
                                     "1\t\t\tProduct 1\t\t\t2\t\t\t100\t\t200\n" +
                                     "2\t\t\tProduct 2\t\t\t1\t\t\t200\t\t200\n" +
                                     "\nTotal Amount to be paid: 400\n";

            // Act
            var receipt = await _repository.GenerateReceiptAsync();

            // Assert
            Assert.That(receipt, Is.EqualTo(expectedReceipt));
        }

        [Test]
        public async Task TransactSaleAsync_ProcessesAndClearsCurrentSale()
        {
            // Arrange
            var product = new Product
            {
                ProductId = 1,
                ProductName = "Product 1",
                ProductType = "Type 1",
                ProductCategory = "Category 1",
                ProductQuantity = 10,
                ProductPrice = 100
            };

            _context.Inventory.Add(product);
            await _context.SaveChangesAsync();

            _repository.GetCurrentSaleForTesting.SaleProducts.Add(new SaleProduct { ProductId = 1, Quantity = 2 });

            // Act
            await _repository.TransactSaleAsync();

            // Assert
            var currentSaleProductsCount = _repository.GetCurrentSaleForTesting.SaleProducts.Count;

            Assert.That(currentSaleProductsCount, Is.EqualTo(0));
        }
    }
}
