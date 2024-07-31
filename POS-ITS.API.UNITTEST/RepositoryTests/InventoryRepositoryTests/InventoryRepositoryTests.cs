using Microsoft.EntityFrameworkCore;
using POS_ITS.DATA;
using POS_ITS.MODEL;
using POS_ITS.REPOSITORIES.InventoryRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_ITS.API.UNITTEST.RepositoryTests.InventoryRepositoryTests
{
    [TestFixture]
    public class InventoryRepositoryTests
    {
        private DbContextOptions<DataDbContext> _dbContextOptions;
        private DataDbContext _context;
        private InventoryRepository _repository;

        [SetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DataDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new DataDbContext(_dbContextOptions);
            _repository = new InventoryRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context?.Dispose();
        }

        [Test]
        public async Task TrackProductQuantityAsync_ReturnsProductQuantity()
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
            var quantity = await _repository.TrackProductQuantityAsync(1);

            //Assert
            Assert.That(quantity, Is.EqualTo(product.ProductQuantity));
        }

        [Test]
        public async Task IncreaseProductQuantityAsync_IncreasesProductQuantityInInventory()
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
            await _repository.IncreaseProductQuantityAsync(1, 5);

            //Assert
            var productQuantityIncreased = await _context.Inventory.FindAsync(1);
            Assert.IsNotNull(productQuantityIncreased);
            Assert.That(productQuantityIncreased.ProductQuantity, Is.EqualTo(15));
        }

        [Test]
        public async Task GetProductPriceAsync_ReturnsProductPrice()
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
            var price = await _repository.GetProductPriceAsync(1);

            //Assert
            Assert.That(price, Is.EqualTo(product.ProductPrice));
        }

        [Test]
        public async Task ChangeProductPriceAsync_ChangesProductPriceInInventory()
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
            await _repository.ChangeProductPriceAsync(1, 250);

            //Assert
            var productQuantityIncreased = await _context.Inventory.FindAsync(1);
            Assert.IsNotNull(productQuantityIncreased);
            Assert.That(productQuantityIncreased.ProductPrice, Is.EqualTo(250));
        }
    }
}
