using Moq;
using POS_ITS.API.MODEL.Entities;
using POS_ITS.REPOSITORIES.ProductRepository;
using POS_ITS.SERVICE.ProductService;

namespace POS_ITS.API.UNITTEST.ServiceTests.ProductServiceTests
{
    [TestFixture]
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _mockProductRepository;
        private ProductService _productService;

        [SetUp]
        public void Setup()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _productService = new ProductService(_mockProductRepository.Object);
        }

        [Test]
        public async Task GetAllProductsAsync_ReturnsProducts()
        {
            // Arrange
            var products = new List<Product>
            {
               new Product { ProductId = 1, ProductName = "Product 1", ProductType = "Type 1", ProductCategory = "Category 1", ProductQuantity = 10, ProductPrice = 100 },
                new Product { ProductId = 2, ProductName = "Product 2", ProductType = "Type 2", ProductCategory = "Category 2", ProductQuantity = 10, ProductPrice = 100  }
            };
            _mockProductRepository.Setup(repo => repo.GetAllProductsAsync())
                .ReturnsAsync(products);

            // Act
            var result = await _productService.GetAllProductsAsync();

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
            _mockProductRepository.Setup(repo => repo.GetProductByIdAsync(1))
                .ReturnsAsync(product);

            //Act 
            var result = await _productService.GetProductByIdAsync(1);

            //Assert
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
            _mockProductRepository.Setup(repo => repo.AddProductAsync(product))
                .Returns(Task.CompletedTask);

            // Act
            await _productService.AddProductAsync(product);

            // Assert
            _mockProductRepository.Verify(repo => repo.AddProductAsync(product), Times.Once);
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

            var updatedProduct = new Product
            {
                ProductId = 1,
                ProductName = "Updated Product 1",
                ProductType = "Updated Type 1",
                ProductCategory = "Updated Category 1",
                ProductQuantity = 20,
                ProductPrice = 200
            };
            _mockProductRepository.Setup(repo => repo.UpdateProductAsync(updatedProduct))
                .Returns(Task.CompletedTask);

            // Act
            await _productService.UpdateProductAsync(updatedProduct);

            // Assert
            // Verify that the UpdateProductAsync method was called once with the updated product
            _mockProductRepository.Verify(r => r.UpdateProductAsync(updatedProduct), Times.Once);
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
            
            // Mock the repository to handle deletion
            _mockProductRepository.Setup(repo => repo.DeleteProductAsync(1))
                .Returns(Task.CompletedTask);

            // Act
            await _productService.DeleteProductAsync(1);

            // Assert
            // Verify that the DeleteProductAsync method was called once with the product ID
            _mockProductRepository.Verify(r => r.DeleteProductAsync(1), Times.Once);
        }
    }
}
