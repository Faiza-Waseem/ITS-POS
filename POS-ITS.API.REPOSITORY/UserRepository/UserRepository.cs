using Microsoft.EntityFrameworkCore;
using POS_ITS.DATA;
using POS_ITS.MODEL;
using System.Security.Claims;
using System.Text;
using System.Data;

namespace POS_ITS.REPOSITORIES.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataDbContext _context;
        private static User CurrentUser { get; set; } = null;

        // Property to expose CurrentUser for testing
        public User GetCurrentUserForTesting => CurrentUser;

        // Helper method to set the CurrentUser for testing purposes
        public void SetCurrentUserForTesting(User user)
        {
            CurrentUser = user;
        }

        public UserRepository(DataDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                return await _context.Users.ToListAsync();
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
                return await _context.Users.FindAsync(id);
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
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => user.Username == u.Username || u.Email == user.Email);

                if (existingUser != null)
                {
                    throw new Exception("User with this username or email already exists.");
                }

                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                throw new Exception($"Repository: Error Registering User: {ex.Message}");
            }
        }

        public async Task LoginAsync(string usernameEmail, string password)
        {
            try
            {
                if (CurrentUser != null)
                {
                    throw new Exception("User already logged in. Log out first!");
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == usernameEmail || u.Email == usernameEmail);

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
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email,
                    Password = user.Password,
                    Role = user.Role
                };
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

                var user = await _context.Users.FindAsync(id);

                if (user == null)
                {
                    throw new Exception($"No user found with id: {id}");
                }

                if (user.UserId == CurrentUser.UserId)
                {
                    throw new Exception("Cannot change your own role.");
                }

                user.Role = role;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Repository: Error changing user role: {ex.Message}");
            }
        }
    }
}
