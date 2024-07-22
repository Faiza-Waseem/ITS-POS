﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ITS_POS.Entities;
using ITS_POS.Data;

namespace ITS_POS.Services
{
    public static class UserAuthentication
    {
        public static User CurrentUser { get; set; } = null;

        public static void RegisterUser(User user)
        {
            DataContext.Users.Add(user);

            Console.WriteLine("User is registered successfully.");
        }

        public static void RegisterUser(string userName, string password, string email)
        {
            DataContext.Users.Add(new User { UserName = userName, Password = password, Email = email });

            Console.WriteLine("User is registered successfully.");
        }

        public static void Login(string userName, string password)
        {
            if (CurrentUser == null)
            {
                var user = DataContext.Users.SingleOrDefault(u => u.UserName == userName && u.Password == password);

                if (user == null)
                {
                    Console.WriteLine("Login failed... Try Again!");
                }
                else
                {
                    CurrentUser = user;

                    Console.WriteLine("User successfully logged in!");
                    Console.WriteLine($"Welcome {user.UserName}");
                }
            }
            else
            {
                Console.WriteLine("You are currently logged in as another user.");
            }
        }

        public static void Logout()
        {
            if (CurrentUser != null)
            {
                CurrentUser = null;

                Console.WriteLine("User logged out successfully.");
            }
            else
            {
                Console.WriteLine("You are not currently logged in.");
            }
        }

        public static void SetUserRole(User user, string role)
        {
            if (CurrentUser != null)
            {
                if (CurrentUser.Role == "Admin")
                {
                    var userFromDB = DataContext.Users.FirstOrDefault(u => u.UserName == user.UserName);

                    if (userFromDB != null)
                    {
                        userFromDB.Role = role;
                    }
                    else
                    {
                        Console.WriteLine($"No user found with username {user.UserName}.");
                    }
                }
                else
                {
                    Console.WriteLine("You are a Cashier. You don't have the access to change user roles.");
                }
            }
            else
            {
                Console.WriteLine("You are not currently logged in.");
            }
        }
    }
}
