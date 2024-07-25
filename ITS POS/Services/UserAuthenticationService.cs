using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ITS_POS.Entities;
using ITS_POS.Data;

namespace ITS_POS.Services
{
    public class UserAuthenticationService : ServiceBase, IUserAuthenticationService
    {
        #region DataMembers

        public static User CurrentUser { get; set; } = null;

        #endregion

        #region Constructor

        public UserAuthenticationService(DataContextDb context) : base(context) { }

        #endregion

        #region Functions
        
        #region Get Context

        public override DataContextDb GetContext()
        {
            return ServiceBase.__context;
        }

        #endregion

        #region User Registration

        public bool RegisterUser(string username, string password, string email, string role)
        {
            bool success = false;

            //var user = DataContext.Users.SingleOrDefault(u => u.Username == username || u.Email == email);
            var user = __context.Users.SingleOrDefault(u => u.Username == username || u.Email == email);

            if (user != null)
            {
                Console.WriteLine("Username or email already exist.");
            }
            else
            {
                User newUser = new User { Username = username, Password = password, Email = email, Role = role };

                //DataContext.Users.Add(newUser);
                __context.Users.Add(newUser);
                __context.SaveChanges();

                Console.WriteLine("User is registered successfully.");
                success = true;
            }

            return success;
        }

        #endregion

        #region User Authentication

        public bool Login(string username, string password)
        {
            bool success = false;

            if (CurrentUser != null)
            {
                Console.WriteLine("You are currently logged in as another user.");
            }
            else
            {
                //var user = DataContext.Users.SingleOrDefault(u => u.Username == username && u.Password == password);
                var user = __context.Users.SingleOrDefault(u => u.Username == username && u.Password == password);

                if (user == null)
                {
                    Console.WriteLine("Login failed... Try Again!");
                }
                else
                {
                    CurrentUser = user;
                    success = true;
                    Console.WriteLine("User successfully logged in!");
                    Console.WriteLine($"Welcome {user.Username}");
                }
            }

            return success;
        }

        public bool Logout()
        {
            bool success = false;

            if (CurrentUser == null)
            {
                Console.WriteLine("You are not currently logged in.");
            }
            else
            {
                CurrentUser = null;
                Console.WriteLine("User logged out successfully.");
                success = true;
            }

            return success;
        }

        #endregion

        #region Set User Role
        public bool SetUserRole(string username, string role)
        {
            bool success = false;

            if (CurrentUser == null)
            {
                Console.WriteLine("You are not currently logged in.");
            }
            else if (CurrentUser.Role != "Admin")
            {
                Console.WriteLine("You are a Cashier. You don't have the access to change user roles.");
            }
            else if (CurrentUser.Username == username)
            {
                Console.WriteLine("You cannot change your own role.");
            }
            else
            {
                //var user = DataContext.Users.SingleOrDefault(u => u.Username == user);
                var user = __context.Users.SingleOrDefault(u => u.Username == username);

                if (user == null)
                {
                    Console.WriteLine($"No user found with username {username}.");
                }
                else
                {
                    user.Role = role;
                    __context.SaveChanges();

                    Console.WriteLine("Role is changed successfully.");
                    success = true;
                }
            }
            return success;
        }

        #endregion

        #region Get All Users

        public List<string> GetAllUsers()
        {
            List<string> users = new List<string>();
            var Users = __context.Users.ToList();

            foreach (var user in Users)
            {
                users.Add(user.ToString());
            }

            return users;
        }

        #endregion

        #endregion
    }
}
