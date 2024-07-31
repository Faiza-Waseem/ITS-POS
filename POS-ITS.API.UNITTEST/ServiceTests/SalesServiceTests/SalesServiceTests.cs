using Moq;
using POS_ITS.MODEL;
using POS_ITS.REPOSITORIES.SalesRepository;
using POS_ITS.SERVICE.SalesService;

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
        public void CalculateAmountForSale_ReturnsCurrentSalesAmount()
        {
            // Arrange
            decimal expectedAmount = 100.00m;
            _mockSalesRepository.Setup(repo => repo.CalculateAmountForSale())
                .Returns(expectedAmount);

            // Act
            decimal actualAmount = _salesService.CalculateAmountForSale();

            // Assert
            Assert.That(actualAmount, Is.EqualTo(expectedAmount));
        }

        [Test]
        public void GenerateReceipt_ReturnsCurrentSaleReceipt()
        {
            // Arrange
            string expectedReceipt = "Sample receipt";
            _mockSalesRepository.Setup(repo => repo.GenerateReceipt())
                .Returns(expectedReceipt);

            // Act
            string actualReceipt = _salesService.GenerateReceipt();

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
