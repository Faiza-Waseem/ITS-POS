using ITS_POS.Entities;
using ITS_POS.Services;
using ITS_POS.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Channels;

namespace ITS_POS_WEB_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryManagementController : ControllerBase
    {
        #region Data Members

        private readonly IInventoryManagement __inventoryManagement;

        #endregion

        #region Constructor

        public InventoryManagementController(IInventoryManagement inventoryManagement)
        {
            __inventoryManagement = inventoryManagement;
        }

        #endregion

        #region Functions

        #region Product Tracking

        #region Product Quantity

        [HttpGet("TrackProductQuantity")]
        public IActionResult TrackProductQuantity([FromQuery] string productName)
        {
            try
            {
                int quantity = -1;
                __inventoryManagement.TrackProductQuantity(productName, out quantity);

                if (quantity != -1)
                {
                    return Ok(quantity);
                }

                return Ok("Error occurred... See Console");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("IncreaseProductQuantity")]
        public IActionResult IncreaseProductQuantity([FromQuery] string productName, [FromQuery] int newQuantity)
        {
            try
            {
                bool api = true;
                __inventoryManagement.IncreaseProductQuantity(productName, newQuantity, out api);

                if (api)
                {
                    return Ok($"Product Quantity is increased by {newQuantity} items.");
                }

                return Ok("Error occurred... See Console");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Product Price

        [HttpGet("CheckProductPrice")]
        public IActionResult CheckProductPrice([FromQuery] string productName)
        {
            try
            {
                decimal price = -1;
                __inventoryManagement.CheckProductPrice(productName, out price);

                if (price != -1)
                {
                    return Ok(price);
                }

                return Ok("Error occurred... See Console");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("SetProductPrice")]
        public IActionResult SetProductPrice([FromQuery] string productName, [FromQuery] decimal newPrice)
        {
            try
            {
                bool api = true;
                __inventoryManagement.SetProductPrice(productName, newPrice, out api);

                if(api)
                {
                    return Ok("Product Price is changed successfully.");
                }

                return Ok("Error occurred... See Console");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #endregion

        #region See Inventory

        [HttpGet("GetInventory")]
        public IActionResult GetInventory()
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