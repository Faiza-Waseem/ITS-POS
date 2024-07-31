using Microsoft.EntityFrameworkCore;
using POS_ITS.REPOSITORIES.ProductRepository;
using POS_ITS.DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_ITS.API.MODEL.Entities;

namespace POS_ITS.API.UNITTEST.RepositoryTests.ProductRepositoryTests
{
    [TestFixture]
    public class ProductRepositoryTests
    {
        private DbContextOptions<DataDbContext> _dbContextOptions;
        private DataDbContext _context;
        private ProductRepository _repository;

        [SetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DataDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new DataDbContext(_dbContextOptions);
            _repository = new ProductRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context?.Dispose();
        }

        [Test]
        public async Task GetAllProductsAsync_ReturnsProducts()
        {
            // Arrange
            _context.Inventory.AddRange(new List<Product>
            {
                new Product { ProductId = 1, ProductName = "Product 1", ProductType = "Type 1", ProductCategory = "Category 1", ProductQuantity = 10, ProductPrice = 100 },
                new Product { ProductId = 2, ProductName = "Product 2", ProductType = "Type 2", ProductCategory = "Category 2", ProductQuantity = 10, ProductPrice = 100  }
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllProductsAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().ProductName, Is.EqualTo("Product 1"));
        }

        [Test]
        public async Task GetProductByIdAsync_ReturnsProduct()
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

            // Act
            var result = await _repository.GetProductByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.ProductName, Is.EqualTo("Product 1"));
        }

        [Test]
        public async Task AddProductAsync_AddsProductToInventory()
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

            // Act
            await _repository.AddProductAsync(product);

            // Assert
            var addedProduct = await _context.Inventory.FindAsync(1);

            Assert.IsNotNull(addedProduct);
            Assert.That(addedProduct.ProductName, Is.EqualTo("Product 1"));
            Assert.That(addedProduct.ProductType, Is.EqualTo("Type 1"));
            Assert.That(addedProduct.ProductCategory, Is.EqualTo("Category 1"));
            Assert.That(addedProduct.ProductQuantity, Is.EqualTo(10));
            Assert.That(addedProduct.ProductPrice, Is.EqualTo(100));
        }

        [Test]
        public async Task UpdateProductAsync_UpdatesProductInInventory()
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

            product.ProductName = "Updated Product 1";
            product.ProductType = "Updated Type 1";
            product.ProductCategory = "Updated Category 1";
            product.ProductQuantity = 20;
            product.ProductPrice = 200;

            //Act
            await _repository.UpdateProductAsync(product);

            //Assert
            var updatedProduct = await _context.Inventory.FindAsync(1);

            Assert.IsNotNull(updatedProduct);
            Assert.That(updatedProduct.ProductName, Is.EqualTo("Updated Product 1"));
            Assert.That(updatedProduct.ProductType, Is.EqualTo("Updated Type 1"));
            Assert.That(updatedProduct.ProductCategory, Is.EqualTo("Updated Category 1"));
            Assert.That(updatedProduct.ProductQuantity, Is.EqualTo(20));
            Assert.That(updatedProduct.ProductPrice, Is.EqualTo(200));
        }

        [Test]
        public async Task DeleteProductAsync_DeletesProductFromInventory()
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
            await _repository.DeleteProductAsync(1);

            //Assert
            var deletedProduct = await _context.Inventory.FindAsync(1);

            Assert.IsNull(deletedProduct);
        }
    }
}
