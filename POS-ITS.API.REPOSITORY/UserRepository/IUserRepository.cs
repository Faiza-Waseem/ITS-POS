using POS_ITS.MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_ITS.REPOSITORIES.UserRepository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task RegisterUserAsync(User user);
        Task LoginAsync(string usernameEmail, string password);
        Task SetUserRoleAsync(int id, string role);
        void Logout();
    }
}
