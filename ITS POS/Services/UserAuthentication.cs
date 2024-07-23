using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ITS_POS.Entities;
using ITS_POS.Data;

namespace ITS_POS.Services
{
    public class UserAuthentication
    {
        #region DataMembers

        public static User CurrentUser { get; set; } = null;
        private static DataContextDb __context = null;

        #endregion

        #region Functions

        #region Get Context

        public static DataContextDb GetContext()
        {
            return __context;
        }

        #endregion

        #region Initialize

        public static void Initialize(DataContextDb context)
        {
            __context = context;
        }

        #endregion

        #region User Registration
        public static void RegisterUser(User newUser, out bool api)
        {
            //var user = DataContext.Users.SingleOrDefault(u => u.Username == newUser.Username);
            var user = __context.Users.SingleOrDefault(u => u.Username == newUser.Username || u.Email == newUser.Email);

            if(user != null)
            {
                Console.WriteLine("Username or email already exist.");
                api = false;
                return;
            }
            //DataContext.Users.Add(newUser);
            __context.Users.Add(newUser);
            __context.SaveChanges();

            Console.WriteLine("User is registered successfully.");
            api = true;
        }

        public static void RegisterUser(User newUser)
        {
            var api = false;
            RegisterUser(newUser, out api);
        }

        public static void RegisterUser(string username, string password, string email, string role)
        {
            User user = new User { Username = username, Password = password, Email = email, Role = role };
            RegisterUser(user);
        }

        #endregion

        #region User Authentication

        public static void Login(string username, string password, out bool api)
        {
            if(CurrentUser != null)
            {
                Console.WriteLine("You are currently logged in as another user.");
                api = false;
                return;
            }

            //var user = DataContext.Users.SingleOrDefault(u => u.Username == username && u.Password == password);
            var user = __context.Users.SingleOrDefault(u => u.Username == username && u.Password == password);
                
            if (user == null)
            {
                Console.WriteLine("Login failed... Try Again!");
                api = false;
                return;
            }
            
            CurrentUser = user;

            Console.WriteLine("User successfully logged in!");
            Console.WriteLine($"Welcome {user.Username}");
            api = true;
        }

        public static void Login(string username, string password)
        {
            var api = false;
            Login(username, password, out api);
        }

        public static void Logout()
        {
            if(CurrentUser == null)
            {
                Console.WriteLine("You are not currently logged in.");
            }

            CurrentUser = null;
            Console.WriteLine("User logged out successfully.");
        }

        #endregion

        #region Set User Role
        public static void SetUserRole(string username, string role, out bool api)
        {
            if (CurrentUser == null)
            {
                Console.WriteLine("You are not currently logged in.");
                api = false;
                return;
            }
            
            if (CurrentUser.Role != "Admin")
            {
                Console.WriteLine("You are a Cashier. You don't have the access to change user roles.");
                api = false;
                return;
            }

            if (CurrentUser.Username == username)
            {
                Console.WriteLine("You cannot change your own role.");
                api = false;
                return;
            }

            //var userFromDB = DataContext.Users.SingleOrDefault(u => u.Username == user);
            var user = __context.Users.SingleOrDefault(u => u.Username == username);

            if(user == null)
            {
                Console.WriteLine($"No user found with username {username}.");
                api = false;
                return;
            }

            user.Role = role;
            Console.WriteLine("Role is changed successfully.");
            api = true;
        }

        public static void SetUserRole(string username, string role)
        {
            var api = false;
            SetUserRole(username, role, out api);
        }

        #endregion

        #endregion
    }
}
