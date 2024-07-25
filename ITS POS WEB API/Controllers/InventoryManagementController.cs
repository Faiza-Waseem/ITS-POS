using ITS_POS.Entities;
using ITS_POS.Services;
using ITS_POS.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Channels;
using Microsoft.AspNetCore.Authorization;

namespace ITS_POS_WEB_API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class InventoryManagementController : ControllerBase
    {
        #region Data Members

        private readonly IInventoryManagementService __inventoryManagement;

        #endregion

        #region Constructor

        public InventoryManagementController(IInventoryManagementService inventoryManagement)
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
                var quantity = __inventoryManagement.TrackProductQuantity(productName);

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
                var success = __inventoryManagement.IncreaseProductQuantity(productName, newQuantity);

                if (success)
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
                var price = __inventoryManagement.CheckProductPrice(productName);

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
                var success = __inventoryManagement.SetProductPrice(productName, newPrice);

                if(success)
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
                var products = __inventoryManagement.GetInventory();

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