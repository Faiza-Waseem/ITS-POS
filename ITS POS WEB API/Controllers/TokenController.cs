using ITS_POS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITS_POS_WEB_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService __tokenService;

        public TokenController(ITokenService tokenService)
        {
            __tokenService = tokenService;
        }

        [HttpPost("Generate Token")]
        [AllowAnonymous]
        public IActionResult GenerateToken()
        {
            try
            {
                var token = __tokenService.GenerateToken();

                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}