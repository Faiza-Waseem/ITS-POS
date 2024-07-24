using ITS_POS.Entities;
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

        private readonly IProductManagement __productManagement;

        #endregion

        #region Constructor

        public ProductManagementController(IProductManagement productManagement)
        {
            __productManagement = productManagement;
        }

        #endregion

        #region Functions

        #region Product Addition To Inventory

        [HttpPost("AddProductToInventoryByObject")]
        public IActionResult AddProductToInventory([FromBody] Product newProduct)
        {
            try
            {
                bool api = true;
                __productManagement.AddProductToInventory(newProduct, out api);

                if (api)
                {
                    return Ok("Product is added successfully.");
                }

                return Ok("Error occurred... See Console");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddProductToInventoryByQuery")]
        public IActionResult AddProductToInventory([FromQuery] string name, [FromQuery] string type, [FromQuery] string category, [FromQuery] int quantity, [FromQuery] decimal price)
        {
            try
            {
                Product newProduct = new Product() { ProductName = name, ProductType = type, ProductCategory = category, ProductQuantity = quantity, ProductPrice = price };
                return AddProductToInventory(newProduct);
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
                bool api = true;
                __productManagement.RemoveProductFromInventory(productName, out api);

                if (api)
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
                Product product = null;
                __productManagement.ViewProductFromInventory(productName, out product);
                
                if (product != null)
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
                bool api = true;
                __productManagement.UpdateProductInInventory(productName, productType, productCategory, productQuantity, productPrice, out api);

                if (api)
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
                var context = ServiceBase.GetContext();
                var products = context.Inventory.ToList<Product>();

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
