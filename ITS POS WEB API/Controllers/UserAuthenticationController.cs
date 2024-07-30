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
using Microsoft.AspNetCore.Authorization;

namespace ITS_POS_WEB_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserAuthenticationController : ControllerBase
    {
        #region Data Members

        private readonly IUserAuthenticationService __userAuthentication;

        #endregion

        #region Constructor

        public UserAuthenticationController(IUserAuthenticationService userAuthentication)
        {
            __userAuthentication = userAuthentication;
        }

        #endregion

        #region Functions

        #region User Registration

        [AllowAnonymous]
        [HttpPost("RegisterUser")]
        public IActionResult RegisterUser([FromQuery] string username, [FromQuery] string password, [FromQuery] string email, [FromQuery] string role)
        {
            try
            {
                var success = __userAuthentication.RegisterUser(username, password, email, role);

                if (success)
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

        #endregion

        #region User Authentication
        
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromQuery] string username, [FromQuery] string password)
        {
            try
            {
                var success = __userAuthentication.Login(username, password);

                if (success)
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
                var success = __userAuthentication.Logout();

                if (success)
                {
                    return Ok("User logged out successfully.");
                }
                return Ok("You are not currently logged in.");
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
                var success = __userAuthentication.SetUserRole(username, role);
                
                if (success)
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
                var users = __userAuthentication.GetAllUsers();

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
