using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS_ITS.MODEL.DTOs.ProductDTOs;
using POS_ITS.MODEL.Entities;
using POS_ITS.SERVICE.ProductService;
using POS_ITS.API.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web.Resource;

namespace POS_ITS.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService service, IMapper mapper, ILogger<ProductController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:UserReadScope")]
        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts()
        {
            try
            {
                _logger.LogInformation("Getting All Products started.");
                var products = await _service.GetAllProductsAsync();
                _logger.LogInformation("Products are displayed successfully.");

                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error: {ex.Message}");
                throw;
                //return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:UserReadScope")]
        [HttpGet("GetProductById")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            try
            {
                _logger.LogInformation("Getting Product with id started.");
                var product = await _service.GetProductByIdAsync(id);
                _logger.LogInformation("Product is displayed successfully.");

                if (product == null)
                {
                    _logger.LogInformation("No product with given id was found.");
                    throw new NotFoundException("No product with given id was found.");
                    //return NotFound();
                }
                return Ok(product);
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
        [HttpPost("AddProductToInventory")]
        public async Task<ActionResult<ProductDTO>> AddProduct([FromBody] ProductDTO productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var product = _mapper.Map<Product>(productDto);

                _logger.LogInformation("Adding Product To Inventory started.");
                await _service.AddProductAsync(product);
                _logger.LogInformation("Product added to inventory successfully.");

                return CreatedAtAction(nameof(GetProductById), new { id = productDto.ProductId }, productDto);
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
        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDTO productDto)
        {
            if (id != productDto.ProductId)
            {
                return BadRequest();
            }

            try
            {
                var product = _mapper.Map<Product>(productDto);

                _logger.LogInformation("Updating Product started.");
                await _service.UpdateProductAsync(product);
                _logger.LogInformation("Product updated successfully.");

                return NoContent();
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
        [HttpDelete("RemoveProductFromInventory")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                _logger.LogInformation("Deleting Product from Inventory started.");
                await _service.DeleteProductAsync(id);
                _logger.LogInformation("Product deleted successfully.");

                return NoContent();
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
