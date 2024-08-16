using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using POS_ITS.MODEL.Entities;
using POS_ITS.MODEL.DTOs.UserDTOs;
using POS_ITS.SERVICE.UserService;
using AutoMapper;
using POS_ITS.MODEL.DTOs.ProductDTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using POS_ITS.API.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web.Resource;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Hosting;

namespace POS_ITS.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IHostEnvironment _environment;
        private readonly IUserService _service;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        ILogger<UserController> _logger;

        public UserController(IUserService service, IMapper mapper, ILogger<UserController> logger, IConfiguration configuration, IHostEnvironment environment)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
            _configuration = configuration;
            _environment = environment;
        }

        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:UserReadScope")]
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
                throw;
                //return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:UserReadScope")]
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
                    throw new NotFoundException("No user with given id was found.");
                    //return NotFound();
                }
                _logger.LogInformation("User is displayed successfully.");
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error: {ex.Message}");
                throw;
                //return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:UserWriteScope")]
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
                throw;
                //return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:UserReadScope")]
        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(string usernameEmail, string password)
        {
            if (string.IsNullOrEmpty(usernameEmail) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Username or password cannot be null or empty.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Login started.");
                var id = await _service.LoginAsync(usernameEmail, password);
                _logger.LogInformation("User logged in successfully.");

                var user = await _service.GetUserByIdAsync(id);

                return Ok($"User logged in successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error: {ex.Message}");
                throw;
                //return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:UserWriteScope")]
        [Authorize(Policy = "AdminPolicy")]
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
                throw;
                //return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:UserReadScope")]
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
                throw;
                //return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"]!,
                audience: _configuration["JwtSettings:Audience"]!,
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
