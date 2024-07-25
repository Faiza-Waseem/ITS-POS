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
    public interface IUserAuthenticationService
    {
        #region DataMembers

        public static User CurrentUser { get; set; }

        #endregion

        #region Functions
        
        #region Get Context

        public DataContextDb GetContext();

        #endregion

        #region User Registration

        public bool RegisterUser(string username, string password, string email, string role);
       
        #endregion

        #region User Authentication

        public bool Login(string username, string password);
        public bool Logout();

        #endregion

        #region Set User Role

        public bool SetUserRole(string username, string role);

        #endregion

        #region Get All Users

        public List<string> GetAllUsers();

        #endregion

        #endregion
    }
}
