using Moq;
using POS_ITS.MODEL;
using POS_ITS.REPOSITORIES.InventoryRepository;
using POS_ITS.SERVICE.InventoryService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_ITS.API.UNITTEST.ServiceTests.InventoryServiceTests
{
    [TestFixture]
    public class InventoryServiceTests
    {
        private Mock<IInventoryRepository> _mockInventoryRepository;
        private InventoryService _inventoryService;

        [SetUp]
        public void Setup()
        {
            _mockInventoryRepository = new Mock<IInventoryRepository>();
            _inventoryService = new InventoryService(_mockInventoryRepository.Object);
        }

        [Test]
        public async Task TrackProductQuantityAsync_ReturnsProductQuantity()
        {
            //Arrange
            int productId = 1;
            int expectedQuantity = 10;

            // Set up the mock repository to return the product quantity
            _mockInventoryRepository.Setup(repo => repo.TrackProductQuantityAsync(productId))
                .ReturnsAsync(expectedQuantity);

            // Act
            var quantity = await _inventoryService.TrackProductQuantityAsync(productId);

            // Assert
            Assert.That(quantity, Is.EqualTo(expectedQuantity));
        }

        [Test]
        public async Task IncreaseProductQuantityAsync_IncreasesProductQuantityInInventory()
        {
            //Arrange
            int productId = 1;
            int quantity = 5;

            // Set up the mock repository to handle increasing the product quantity
            _mockInventoryRepository.Setup(repo => repo.IncreaseProductQuantityAsync(productId, quantity))
                .Returns(Task.CompletedTask);

            // Act
            await _inventoryService.IncreaseProductQuantityAsync(productId, quantity);

            // Assert
            _mockInventoryRepository.Verify(repo => repo.IncreaseProductQuantityAsync(productId, quantity), Times.Once);
        }

        [Test]
        public async Task GetProductPriceAsync_ReturnsProductPrice()
        {
            // Arrange
            int productId = 1;
            decimal expectedPrice = 100m;
            // Set up the mock repository to return the product price
            _mockInventoryRepository.Setup(repo => repo.GetProductPriceAsync(productId))
                .ReturnsAsync(expectedPrice);

            // Act
            var price = await _inventoryService.GetProductPriceAsync(productId);

            // Assert
            Assert.That(price, Is.EqualTo(expectedPrice));
        }

        [Test]
        public async Task ChangeProductPriceAsync_ChangesProductPriceInInventory()
        {
            // Arrange
            int productId = 1;
            decimal price = 250m;
            
            // Set up the mock repository to handle changing the product price
            _mockInventoryRepository.Setup(repo => repo.ChangeProductPriceAsync(productId, price))
                .Returns(Task.CompletedTask);

            // Act
            await _inventoryService.ChangeProductPriceAsync(productId, price);

            // Assert
            _mockInventoryRepository.Verify(repo => repo.ChangeProductPriceAsync(productId, price), Times.Once);
        }
    }
}
