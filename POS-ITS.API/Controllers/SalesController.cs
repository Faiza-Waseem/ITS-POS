using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS_ITS.SERVICE.SalesService;

namespace POS_ITS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISalesService _service;

        public SalesController(ISalesService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Cashier")]
        [HttpPost("AddProductToCurrentSale")]
        public async Task<ActionResult> AddProductToSaleAsync(int id, int quantity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _service.AddProductToSaleAsync(id, quantity);
                return Ok("Product added to current sale successfully.");
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Cashier")]
        [HttpGet("CalculateAmountForCurrentSale")]
        public async Task<ActionResult<decimal>> CalculateAmountForSale()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var amount = _service.CalculateAmountForSale();
                return Ok(amount);
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Cashier")]
        [HttpGet("GenerateReceiptForCurrentSale")]
        public async Task<ActionResult<string>> GenerateReceipt()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var receipt = _service.GenerateReceipt();
                return Ok(receipt);
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = "Cashier")]
        [HttpGet("TransactCurrentSale")]
        public async Task<ActionResult> TransactSaleAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _service.TransactSaleAsync();
                return Ok("Current Sale Transacted successfully.");
            }
            catch (Exception ex)
            {
                // Log exception (logging code can be added here)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
