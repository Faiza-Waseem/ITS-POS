using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS_ITS.MODEL;
using POS_ITS.SERVICE.InventoryService;

namespace POS_ITS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _service;

        public InventoryController(IInventoryService service)
        {
            _service = service;
        }

        [HttpGet("TrackProductQuantity")]
        public async Task<ActionResult<int>> TrackProductQuantityAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var quantity = await _service.TrackProductQuantityAsync(id);
                return Ok(quantity);
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("IncreaseProductQuantity")]
        public async Task<ActionResult> IncreaseProductQuantityAsync(int id, int quantity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _service.IncreaseProductQuantityAsync(id, quantity);
                return Ok("Product Quantity increased successfully.");
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("CheckProductPrice")]
        public async Task<ActionResult<decimal>> GetProductPriceAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var price = await _service.GetProductPriceAsync(id);
                return Ok(price);
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("ChangeProductPrice")]
        public async Task<ActionResult> ChangeProductPriceAsync(int id, decimal price)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _service.ChangeProductPriceAsync(id, price);
                return Ok("Product Price changed successfully.");
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
