using ITS_POS.Data;
using ITS_POS.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS_POS.Services
{
    public interface IUserAuthentication
    {
        #region DataMembers

        public static User CurrentUser { get; set; }

        #endregion

        #region Functions

        #region User Registration
        public void RegisterUser(User newUser, out bool api);
        public void RegisterUser(User newUser);
        public void RegisterUser(string username, string password, string email, string role);

        #endregion

        #region User Authentication

        public void Login(string username, string password, out bool api);
        public void Login(string username, string password);
        public void Logout();

        #endregion

        #region Set User Role
        public void SetUserRole(string username, string role, out bool api);
        public void SetUserRole(string username, string role);

        #endregion

        #endregion
    }
}
