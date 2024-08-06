using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using POS_ITS.REPOSITORIES.UserRepository;
using User = POS_ITS.MODEL.Entities.User;

namespace POS_ITS.REPOSITORIES.UserRepository
{
    public class UserCosmosRepository : IUserRepository
    {
        private readonly Container _userContainer;

        private static User CurrentUser { get; set; } = null;

        public UserCosmosRepository(CosmosClient client, string databaseName)
        {
            var userContainerName = "Users";
            _userContainer = client.GetContainer(databaseName, userContainerName);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                var query = new QueryDefinition("SELECT * FROM c");
                var users = new List<User>();

                using (var feedIterator = _userContainer.GetItemQueryIterator<User>(query))
                {
                    while (feedIterator.HasMoreResults)
                    {
                        var response = await feedIterator.ReadNextAsync();
                        users.AddRange(response);
                    }
                }

                return users;
            }
            catch (CosmosException cosmosEx)
            {
                throw new Exception($"CosmosDB Error: {cosmosEx.StatusCode} - {cosmosEx.Message}");
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                throw new Exception($"Repository: Error getting all users: {ex.Message}");
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                var userId = "user_" + id.ToString();
                var user = await _userContainer.ReadItemAsync<User>(userId, new PartitionKey(id));
                return user.Resource;
            }
            catch (CosmosException cosmosEx)
            {
                throw new Exception($"CosmosDB Error: {cosmosEx.StatusCode} - {cosmosEx.Message}");
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                throw new Exception($"Repository: Error getting user by id {id}: {ex.Message}");
            }
        }

        public async Task RegisterUserAsync(User user)
        {
            try
            {
                var query = new QueryDefinition("SELECT * FROM c WHERE c.Username = @username OR c.Email = @email")
                .WithParameter("@username", user.Username)
                .WithParameter("@email", user.Email);

                var existingUser = false;
                using (var feedIterator = _userContainer.GetItemQueryIterator<User>(query))
                {
                    while (feedIterator.HasMoreResults)
                    {
                        var response = await feedIterator.ReadNextAsync();
                        existingUser = response.Any();
                    }
                }

                if (existingUser)
                {
                    throw new Exception("User with this username or email already exists.");
                }

                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                await _userContainer.CreateItemAsync(user, new PartitionKey(user.UserId));
            }
            catch (CosmosException cosmosEx)
            {
                throw new Exception($"CosmosDB Error: {cosmosEx.StatusCode} - {cosmosEx.Message}");
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                throw new Exception($"Repository: Error Registering User: {ex.Message}");
            }
        }

        public async Task<int> LoginAsync(string usernameEmail, string password)
        {
            try
            {
                if (CurrentUser != null)
                {
                    throw new Exception("User already logged in. Log out first!");
                }

                var query = new QueryDefinition("SELECT * FROM c WHERE c.Username = @usernameEmail OR c.Email = @usernameEmail")
                    .WithParameter("@usernameEmail", usernameEmail);

                User user = null;
                using (var feedIterator = _userContainer.GetItemQueryIterator<User>(query))
                {
                    while (feedIterator.HasMoreResults)
                    {
                        var response = await feedIterator.ReadNextAsync();
                        user = response.FirstOrDefault();
                    }
                }

                if (user == null)
                {
                    throw new Exception($"No user exists with {usernameEmail} username or email.");
                }

                if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    throw new Exception("Password is wrong.");
                }

                CurrentUser = new User
                {
                    Id = user.Id,
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email,
                    Password = user.Password,
                    Role = user.Role
                };

                return CurrentUser.UserId;
            }
            catch (CosmosException cosmosEx)
            {
                throw new Exception($"CosmosDB Error: {cosmosEx.StatusCode} - {cosmosEx.Message}");
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                throw new Exception($"Repository: Error logging in: {ex.Message}");
            }
        }

        public void Logout()
        {
            try
            {
                if (CurrentUser == null)
                {
                    throw new Exception("No user logged in.");
                }

                CurrentUser = null;
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                throw new Exception($"Repository: Error logging out: {ex.Message}");
            }
        }

        public async Task SetUserRoleAsync(int id, string role)
        {
            try
            {
                if (CurrentUser == null)
                {
                    throw new Exception("No user logged in.");
                }

                if (CurrentUser.Role == "Cashier")
                {
                    throw new Exception("Cashier cannot change user role.");
                }

                var user = await GetUserByIdAsync(id);
                
                if (user == null)
                {
                    throw new Exception($"No user found with id: {id}");
                }

                if (user.UserId == CurrentUser.UserId)
                {
                    throw new Exception("Cannot change your own role.");
                }

                if(user.Role == role)
                {
                    return;
                }

                user.Role = role;
                await _userContainer.ReplaceItemAsync(user, "user_" + id.ToString(), new PartitionKey(id));
            }
            catch (CosmosException cosmosEx)
            {
                throw new Exception($"CosmosDB Error: {cosmosEx.StatusCode} - {cosmosEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Repository: Error changing user role: {ex.Message}");
            }
        }
    }
}
