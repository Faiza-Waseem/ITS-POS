using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using POS_ITS.MODEL.Entities;
using POS_ITS.MODEL.DTOs.UserDTOs;
using POS_ITS.SERVICE.UserService;
using AutoMapper;
using POS_ITS.MODEL.DTOs.ProductDTOs;

namespace POS_ITS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;
        ILogger<UserController> _logger;

        public UserController(IUserService service, IMapper mapper, ILogger<UserController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            try
            {
                _logger.LogInformation("Getting All Users started");
                var users = await _service.GetAllUsersAsync();
                _logger.LogInformation("Users are displayed successfully.");

                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetUserById")]
        public async Task<ActionResult<UserDTO>> GetUserById(int id)
        {
            try
            {
                _logger.LogInformation("Getting user by id started.");
                var user = await _service.GetUserByIdAsync(id);
                if (user == null)
                {
                    _logger.LogInformation("No user with given id was found.");
                    return NotFound();
                }
                _logger.LogInformation("User is displayed successfully.");
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("RegisterUser")]
        public async Task<ActionResult> RegisterUserAsync(UserDTO userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = _mapper.Map<User>(userDto);

                _logger.LogInformation("Registering User started."); 
                await _service.RegisterUserAsync(user);
                _logger.LogInformation("User is registered successfully.");
                
                return CreatedAtAction(nameof(GetUserById), new { id = userDto.UserId }, userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error: {ex.Message}");
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
                _logger.LogInformation("Login started.");
                await _service.LoginAsync(usernameEmail, password);
                _logger.LogInformation("User logged in successfully.");

                return Ok("User logged in successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error: {ex.Message}");
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
                _logger.LogInformation("Changing User Role started.");
                await _service.SetUserRoleAsync(id, role);
                _logger.LogInformation("Role for the user with id changed successfully.");

                return Ok("Role changed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error: {ex.Message}");
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
                _logger.LogInformation("User logging out started.");
                _service.Logout();
                _logger.LogInformation("User logged out successfully.");

                return Ok("User logged out successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
