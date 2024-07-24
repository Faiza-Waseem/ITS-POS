using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ITS_POS.Data;
using ITS_POS.Entities;
using ITS_POS.Services;
using Microsoft.AspNetCore.Authentication;

namespace ITS_POS_WEB_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserAuthenticationController : ControllerBase
    {
        #region Data Members

        private readonly IUserAuthentication __userAuthentication;

        #endregion

        #region Constructor

        public UserAuthenticationController(IUserAuthentication userAuthentication)
        {
            __userAuthentication = userAuthentication;
        }

        #endregion

        #region Functions

        #region User Registration

        [HttpPost("RegisterUserByObject")]
        public IActionResult RegisterUser([FromBody]User newUser)
        {
            try
            {
                bool api = true;
                __userAuthentication.RegisterUser(newUser, out api);

                if(api)
                {
                    return Ok("User is registered successfully.");
                }
                
                return Ok("Error occurred... See Console");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("RegisterUserByQuery")]
        public IActionResult RegisterUser([FromQuery] string username, [FromQuery] string password, [FromQuery] string email, [FromQuery] string role)
        {
            try
            {
                User newUser = new User() { Username = username, Password = password, Email = email, Role = role };
                return RegisterUser(newUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region User Authentication

        [HttpPost("Login")]
        public IActionResult Login([FromQuery] string username, [FromQuery] string password)
        {
            try
            {
                bool api = true;
                __userAuthentication.Login(username, password, out api);
                if (api)
                {
                    return Ok("User successfully logged in.");
                }
                
                return Ok("Error occurred... See Console");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            try
            {
                if (UserAuthentication.CurrentUser != null)
                {
                    __userAuthentication.Logout();
                    return Ok("User logged out successfully.");
                }
                else
                {
                    return Ok("You are not currently logged in.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Set User Role

        [HttpPost("SetUserRole")]
        public IActionResult SetUserRole([FromQuery] string username, [FromQuery] string role)
        {
            try
            {
                bool api = true;

                __userAuthentication.SetUserRole(username, role, out api);
                if (api)
                {
                    return Ok("Role is changed successfully.");
                }

                return Ok("Error occurred... See Console");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Get Users

        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            try
            {
                var context = ServiceBase.GetContext();
                var users = context.Users.ToList<User>();

                return Ok(users);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }

        #endregion

        #endregion
    }
}
