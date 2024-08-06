using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using POS_ITS.MODEL;
using POS_ITS.SERVICE.InventoryService;

namespace POS_ITS.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _service;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(IInventoryService service, ILogger<InventoryController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:UserReadScope")]
        [HttpGet("TrackProductQuantity")]
        public async Task<ActionResult<int>> TrackProductQuantityAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Tracking Product Quantity started.");
                var quantity = await _service.TrackProductQuantityAsync(id);
                _logger.LogInformation($"Product Quantity tracked successfully.");

                return Ok(quantity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error: {ex.Message}");
                throw;
                //return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(AuthenticationSchemes = "CustomJWTBearer" ,Roles = "Admin")]
        [HttpPost("IncreaseProductQuantity")]
        public async Task<ActionResult> IncreaseProductQuantityAsync(int id, int quantity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Increase Product Quantity started.");
                await _service.IncreaseProductQuantityAsync(id, quantity);
                _logger.LogInformation("Product Quantity increased successfully.");

                return Ok("Product Quantity increased successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error: {ex.Message}");
                throw;
                //return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:UserReadScope")]
        [HttpGet("CheckProductPrice")]
        public async Task<ActionResult<decimal>> GetProductPriceAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Get Product Price started.");
                var price = await _service.GetProductPriceAsync(id);
                _logger.LogInformation("Product Price checked successfully.");
                
                return Ok(price);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error: {ex.Message}");
                throw;
                //return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(AuthenticationSchemes = "CustomJWTBearer", Roles = "Admin")]
        [HttpPost("ChangeProductPrice")]
        public async Task<ActionResult> ChangeProductPriceAsync(int id, decimal price)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Changing Product Price started.");
                await _service.ChangeProductPriceAsync(id, price);
                _logger.LogInformation("Product Price changed successfully.");

                return Ok("Product Price changed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error: {ex.Message}");
                throw;
                //return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
