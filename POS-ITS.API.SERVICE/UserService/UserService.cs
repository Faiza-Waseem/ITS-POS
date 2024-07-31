using POS_ITS.MODEL.Entities;
using POS_ITS.REPOSITORIES.UserRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_ITS.SERVICE.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                return await _repository.GetAllUsersAsync();
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                throw new Exception($"Service: Error getting all users: {ex.Message}");
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                return await _repository.GetUserByIdAsync(id);
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                throw new Exception($"Service: Error getting user by id {id}: {ex.Message}");
            }
        }

        public async Task RegisterUserAsync(User user)
        {
            try
            {
                await _repository.RegisterUserAsync(user);
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                throw new Exception($"Service: Error registering user: {ex.Message}");
            }
        }

        public async Task<int> LoginAsync(string usernameEmail, string password)
        {
            try
            {
                return await _repository.LoginAsync(usernameEmail, password);
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                throw new Exception($"Service: Error logging in: {ex.Message}");
            }
        }

        public async Task SetUserRoleAsync(int id, string role)
        {
            try
            {
                await _repository.SetUserRoleAsync(id, role);
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                throw new Exception($"Service: Error setting {role} for user with id {id}: {ex.Message}");
            }
        }

        public void Logout()
        {
            try
            {
                _repository.Logout();
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                throw new Exception($"Service: Error logging out: {ex.Message}");
            }
        }
    }
}
