using Microsoft.EntityFrameworkCore;
using POS_ITS.MODEL.Entities;
using POS_ITS.DATA;
using POS_ITS.REPOSITORIES.UserRepository;

namespace POS_ITS.API.UNITTEST.RepositoryTests.UserRepositoryTests
{
    public class UserRepositoryTests
    {
        private DbContextOptions<DataDbContext> _dbContextOptions;
        private DataDbContext _context;
        private UserRepository _repository;

        [SetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DataDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new DataDbContext(_dbContextOptions);
            _repository = new UserRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context?.Dispose();
        }

        [Test]
        public async Task GetAllUsersAsync_ReturnsUsers()
        {
            // Arrange
            _context.Users.AddRange(new List<User>
            {
               new User {UserId = 1, Username = "User 1", Email = "user1@gmail.com", Password =  "abc123", Role = "Admin"},
               new User {UserId = 2, Username = "User 2", Email = "user2@gmail.com", Password =  "def456", Role = "Cashier"}
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllUsersAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Username, Is.EqualTo("User 1"));
        }

        [Test]
        public async Task GetUserByIdAsync_ReturnsUser()
        {
            //Arrange
            var user = new User
            {
                UserId = 1,
                Username = "User 1",
                Email = "user1@gmail.com",
                Password = "abc123",
                Role = "Admin"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetUserByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Username, Is.EqualTo("User 1"));
        }

        [Test]
        public async Task RegisterUserAsync_AddsUserToUsers()
        {
            //Arrange
            var user = new User
            {
                UserId = 1,
                Username = "User 1",
                Email = "user1@gmail.com",
                Password = "abc123",
                Role = "Admin"
            };

            // Act
            await _repository.RegisterUserAsync(user);

            // Assert
            var registeredUser = await _context.Users.FindAsync(1);

            Assert.IsNotNull(registeredUser);
            Assert.That(registeredUser.Username, Is.EqualTo("User 1"));
            Assert.That(registeredUser.Email, Is.EqualTo("user1@gmail.com"));
            Assert.That(registeredUser.Role, Is.EqualTo("Admin"));
        }

        [Test]
        public async Task LoginAsync_SetsCurrentUser()
        {
            //Arrange
            var user = new User
            {
                UserId = 1,
                Username = "User 1",
                Email = "user1@gmail.com",
                Password = BCrypt.Net.BCrypt.HashPassword("abc123"),
                Role = "Admin"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            //Act
            await _repository.LoginAsync("User 1", "abc123");

            //Assert
            var loggedInUser = _repository.GetCurrentUserForTesting;

            Assert.IsNotNull(loggedInUser);
            Assert.That(loggedInUser.Username, Is.EqualTo($"{user.Username}"));
            Assert.That(loggedInUser.Email, Is.EqualTo($"{user.Email}"));
            Assert.That(loggedInUser.Role, Is.EqualTo($"{user.Role}"));
        }

        [Test]
        public void Logout_WhenUserIsLoggedIn_SetsCurrentUserToNull()
        {
            // Arrange
            var user = new User
            {
                UserId = 1,
                Username = "User 1",
                Password = "abc123",
                Email = "user1@gmail.com",
                Role = "Admin"
            };

            _repository.SetCurrentUserForTesting(user);  

            // Act
            _repository.Logout();

            // Assert
            Assert.IsNull(_repository.GetCurrentUserForTesting);
        }

        [Test]
        public void Logout_WhenNoUserIsLoggedIn_ThrowsException()
        {
            // Arrange
            _repository.SetCurrentUserForTesting(null);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _repository.Logout());
            Assert.That(ex.Message, Is.EqualTo("No user logged in."));
        }

        [Test]
        public async Task SetUserRoleAsync_ThrowsException_WhenNoUserLoggedIn()
        {
            // Arrange
            var user = new User
            {
                UserId = 1,
                Username = "User 1",
                Password = "abc123",
                Email = "user1@gmail.com",
                Role = "Admin"
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _repository.SetUserRoleAsync(1, "Cashier"));
            Assert.That(ex.Message, Is.EqualTo("No user logged in."));
        }

        [Test]
        public async Task SetUserRoleAsync_ThrowsException_WhenCurrentUserIsCashier()
        {
            // Arrange
            var currentUser = new User
            {
                UserId = 2,
                Username = "User 2",
                Password = "def456",
                Email = "user2@gmail.com",
                Role = "Cashier"
            };
            _repository.SetCurrentUserForTesting(currentUser);

            var user = new User
            {
                UserId = 1,
                Username = "User 1",
                Password = "abc123",
                Email = "user1@gmail.com",
                Role = "Admin"
            };
            await _context.Users.AddRangeAsync(user, currentUser);
            await _context.SaveChangesAsync();

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _repository.SetUserRoleAsync(1, "Cashier"));
            Assert.That(ex.Message, Is.EqualTo("Cashier cannot change user role."));
        }

        [Test]
        public async Task SetUserRoleAsync_ThrowsException_WhenUserNotFound()
        {
            // Arrange
            var currentUser = new User
            {
                UserId = 2,
                Username = "User 2",
                Password = "def456",
                Email = "user2@gmail.com",
                Role = "Admin"
            };
            _repository.SetCurrentUserForTesting(currentUser);

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _repository.SetUserRoleAsync(1, "Admin"));
            Assert.That(ex.Message, Is.EqualTo("No user found with id: 1"));
        }

        [Test]
        public async Task SetUserRoleAsync_ThrowsException_WhenChangingOwnRole()
        {
            // Arrange
            var currentUser = new User
            {
                UserId = 2,
                Username = "User 2",
                Password = "def456",
                Email = "user2@gmail.com",
                Role = "Admin"
            };
            _repository.SetCurrentUserForTesting(currentUser);

            await _context.Users.AddAsync(currentUser);
            await _context.SaveChangesAsync();

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(async () => await _repository.SetUserRoleAsync(2, "Cashier"));
            Assert.That(ex.Message, Is.EqualTo("Cannot change your own role."));
        }

        [Test]
        public async Task SetUserRoleAsync_ChangesRole_WhenValid()
        {
            // Arrange
            var currentUser = new User
            {
                UserId = 2,
                Username = "User 2",
                Password = "def456",
                Email = "user2@gmail.com",
                Role = "Admin"
            };
            _repository.SetCurrentUserForTesting(currentUser);

            var user = new User
            {
                UserId = 1,
                Username = "User 1",
                Password = "abc123",
                Email = "user1@gmail.com",
                Role = "Cashier"
            };
            await _context.Users.AddRangeAsync(user, currentUser);
            await _context.SaveChangesAsync();

            // Act
            await _repository.SetUserRoleAsync(1, "Admin");

            // Assert
            var updatedUser = await _context.Users.FindAsync(1);
            Assert.IsNotNull(updatedUser);
            Assert.That(updatedUser.Role, Is.EqualTo("Admin"));
        }
    }
}
