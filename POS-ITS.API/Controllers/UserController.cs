using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using POS_ITS.MODEL;
using POS_ITS.SERVICE.UserService;

namespace POS_ITS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            try
            {
                var users = await _service.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetUserById")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            try
            {
                var user = await _service.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("RegisterUser")]
        public async Task<ActionResult> RegisterUserAsync(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _service.RegisterUserAsync(user);
                return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(string usernameEmail, string password)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _service.LoginAsync(usernameEmail, password);
                return Ok("User logged in successfully.");
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("ChangeUserRole")]
        public async Task<ActionResult> SetUserRoleAsync(int id, string role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _service.SetUserRoleAsync(id, role);
                return Ok("Role changed successfully.");
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("Logout")]
        public async Task<ActionResult> Logout()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _service.Logout();
                return Ok("User logged out successfully.");
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
