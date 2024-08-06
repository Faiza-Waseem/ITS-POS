using Moq;
using POS_ITS.MODEL.Entities;
using POS_ITS.REPOSITORIES.SalesRepository;
using POS_ITS.SERVICE.SalesService;
using System.Runtime.CompilerServices;

namespace POS_ITS.API.UNITTEST.ServiceTests.SalesServiceTests
{
    [TestFixture]
    public class SalesServiceTests
    {
        private Mock<ISalesRepository> _mockSalesRepository;
        private SalesService _salesService;

        [SetUp]
        public void Setup()
        {
            _mockSalesRepository = new Mock<ISalesRepository>();
            _salesService = new SalesService(_mockSalesRepository.Object);
        }

        [Test]
        public async Task AddProductToSaleAsync_AddsProductToCurrentSale()
        {
            //Arrange
            int productId = 1;
            int quantity = 5;

            _mockSalesRepository.Setup(repo => repo.AddProductToSaleAsync(productId, quantity))
            .Returns(Task.CompletedTask);

            //Act
            await _salesService.AddProductToSaleAsync(productId, quantity);

            //Assert
            _mockSalesRepository.Verify(repo => repo.AddProductToSaleAsync(productId, quantity), Times.Once());
        }

        [Test]
        public async Task CalculateAmountForSaleAsync_ReturnsCurrentSalesAmount()
        {
            // Arrange
            decimal expectedAmount = 0m;
            Moq.Language.Flow.ISetup<ISalesRepository, Task<decimal>> setup = _mockSalesRepository.Setup(repo => repo.CalculateAmountForSaleAsync());

            // Act
            decimal actualAmount = await _salesService.CalculateAmountForSaleAsync();

            // Assert
            Assert.That(actualAmount, Is.EqualTo(expectedAmount));
        }

        [Test]
        public async Task GenerateReceiptAsync_ReturnsCurrentSaleReceipt()
        {
            // Arrange
            string expectedReceipt = null;
            Moq.Language.Flow.ISetup<ISalesRepository, Task<string>> setup = _mockSalesRepository.Setup(repo => repo.GenerateReceiptAsync());

            // Act
            string actualReceipt = await _salesService.GenerateReceiptAsync();

            // Assert
            Assert.That(actualReceipt, Is.EqualTo(expectedReceipt));
        }

        [Test]
        public async Task TransactSaleAsync_ProcessesAndClearsCurrentSale()
        {
            // Arrange
            _mockSalesRepository.Setup(repo => repo.TransactSaleAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _salesService.TransactSaleAsync();

            // Assert
            _mockSalesRepository.Verify(repo => repo.TransactSaleAsync(), Times.Once);
        }

    }
}
