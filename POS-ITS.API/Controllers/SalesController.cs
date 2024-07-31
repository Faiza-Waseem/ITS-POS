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
        private readonly ILogger<SalesController> _logger;

        public SalesController(ISalesService service, ILogger<SalesController> logger)
        {
            _service = service;
            _logger = logger;
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
                _logger.LogInformation("Adding product to current sale started.");
                await _service.AddProductToSaleAsync(id, quantity);
                _logger.LogInformation("Product added to current sale successfully");

                return Ok("Product added to current sale successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error: {ex.Message}");
                throw;
                //return StatusCode(500, $"Internal server error: {ex.Message}");
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
                _logger.LogInformation("Calculating amount for current sale started.");
                var amount = _service.CalculateAmountForSale();
                _logger.LogInformation("Amount calculated for current sale successfully.");

                return Ok(amount);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error: {ex.Message}");
                throw;
                //return StatusCode(500, $"Internal server error: {ex.Message}");
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
                _logger.LogInformation("Generating Receipt for current sale started.");
                var receipt = _service.GenerateReceipt();
                _logger.LogInformation("Receipt for current sale generated successfully.");

                return Ok(receipt);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error: {ex.Message}");
                throw;
                //return StatusCode(500, $"Internal server error: {ex.Message}");
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
                _logger.LogInformation("Transacting current sale started.");
                await _service.TransactSaleAsync();
                _logger.LogInformation("Current sale transacted successfully.");

                return Ok("Current Sale Transacted successfully.");
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
