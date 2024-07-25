using ITS_POS.Services;
using ITS_POS.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Channels;

namespace ITS_POS_WEB_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductManagementController : ControllerBase
    {
        #region Data Members

        private readonly IProductManagementService __productManagement;

        #endregion

        #region Constructor

        public ProductManagementController(IProductManagementService productManagement)
        {
            __productManagement = productManagement;
        }

        #endregion

        #region Functions

        #region Product Addition To Inventory

        [HttpPost("AddProduct")]
        public IActionResult AddProductToInventory([FromQuery] string name, [FromQuery] string type, [FromQuery] string category, [FromQuery] int quantity, [FromQuery] decimal price)
        {
            try
            {
                var success = __productManagement.AddProductToInventory(name, type, category, quantity, price);

                if (success)
                {
                    return Ok("Product is added successfully.");
                }

                return Ok("Error occurred... See Console");
                //Unauthorized("some error");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Product Removal From Inventory

        [HttpPost("RemoveProductFromInventory")]
        public IActionResult RemoveProductFromInventory([FromQuery] string productName)
        {
            try
            {
                var success = __productManagement.RemoveProductFromInventory(productName);

                if (success)
                {
                    return Ok("Product Removed from the Inventory.");
                }

                return Ok("Error occurred... See Console");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region View Product From Inventory

        [HttpGet("ViewProductFromInventory")]
        public IActionResult ViewProductFromInventory([FromQuery] string productName)
        {
            try
            {
                var product = __productManagement.ViewProductFromInventory(productName);
                
                if (product != "")
                {
                    return Ok(product);
                }

                return Ok("Error occurred... See Console");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Update Product In Inventory

        [HttpPost("UpdateProductInInventory")]
        public IActionResult UpdateProductInInventory([FromQuery] string productName, string productType = "", string productCategory = "", int productQuantity = 0, decimal productPrice = 0m)
        {
            try
            {
                var success = __productManagement.UpdateProductInInventory(productName, productType, productCategory, productQuantity, productPrice);

                if (success)
                {
                    return Ok("Product updated successfully.");
                }

                return Ok("Error occurred... See Console");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Get Products

        [HttpGet("GetAllProducts")]
        public IActionResult GetAllProducts()
        {
            try
            {
                var products = __productManagement.GetAllProducts();

                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion
        
        #endregion
    }
}
