using Moq;
using POS_ITS.MODEL.Entities;
using POS_ITS.REPOSITORIES.SalesRepository;
using POS_ITS.REPOSITORIES.UserRepository;
using POS_ITS.SERVICE.UserService;

namespace POS_ITS.API.UNITTEST.ServiceTests.UserServiceTests
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _mockUserRepository;
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepository.Object);
        }

        [Test]
        public async Task GetAllUsersAsync_ReturnsUsers()
        {
            // Arrange
            var users = new List<User>
            {
               new User {UserId = 1, Username = "User 1", Email = "user1@gmail.com", Password = "abc123", Role = "Admin"},
               new User {UserId = 2, Username = "User 2", Email = "user2@gmail.com", Password =  "def456", Role = "Cashier"}
            };

            _mockUserRepository.Setup(repo => repo.GetAllUsersAsync())
                .ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(users));
        }

        [Test]
        public async Task GetUserByIdAsync_ReturnsUser()
        {
            // Arrange
            var user = new User
            {
                UserId = 1,
                Username = "User 1",
                Email = "user1@gmail.com",
                Password = "abc123",
                Role = "Admin"
            };

            _mockUserRepository.Setup(repo => repo.GetUserByIdAsync(user.UserId))
                .ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserByIdAsync(user.UserId);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result, Is.EqualTo(user));
        }

        [Test]
        public async Task RegisterUserAsync_AddsUserToUsers()
        {
            // Arrange
            var user = new User
            {
                UserId = 1,
                Username = "User 1",
                Email = "user1@gmail.com",
                Password = "abc123",
                Role = "Admin"
            };

            _mockUserRepository.Setup(repo => repo.RegisterUserAsync(user))
                .Returns(Task.CompletedTask);

            // Act
            await _userService.RegisterUserAsync(user);

            // Assert
            _mockUserRepository.Verify(repo => repo.RegisterUserAsync(user), Times.Once);
        }

        [Test]
        public async Task LoginAsync_SetsCurrentUser()
        {
            // Arrange
            var usernameEmail = "user@example.com";
            var password = "password";
            Moq.Language.Flow.ISetup<IUserRepository, Task<int>> setup = _mockUserRepository.Setup(repo => repo.LoginAsync(usernameEmail, password));

            // Act
            await _userService.LoginAsync(usernameEmail, password);

            // Assert
            _mockUserRepository.Verify(repo => repo.LoginAsync(usernameEmail, password), Times.Once);
        }

        [Test]
        public async Task SetUserRoleAsync_SetsUserRole()
        {
            // Arrange
            var userId = 1;
            var newRole = "Admin";

            _mockUserRepository.Setup(repo => repo.SetUserRoleAsync(userId, newRole))
                .Returns(Task.CompletedTask);

            // Act
            await _userService.SetUserRoleAsync(userId, newRole);

            // Assert
            _mockUserRepository.Verify(repo => repo.SetUserRoleAsync(userId, newRole), Times.Once);
        }

        [Test]
        public void Logout_CallsLogoutInRepository()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.Logout())
                .Verifiable();

            // Act
            _userService.Logout();

            // Assert
            _mockUserRepository.Verify(repo => repo.Logout(), Times.Once);
        }
    }
}
