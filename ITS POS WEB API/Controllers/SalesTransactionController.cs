using ITS_POS.Entities;
using ITS_POS.Services;
using ITS_POS.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Channels;

namespace ITS_POS_WEB_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesTransactionController : ControllerBase
    {
        #region Constructor

        public SalesTransactionController() { }

        #endregion

        #region Functions

        #region Product Addition to Sale

        [HttpPost("AddProductToSale")]
        public IActionResult AddProductToSale([FromQuery] String productName, [FromQuery] int quantity)
        {
            try
            {
                bool api = true;
                SalesTransaction.AddProductToSale(productName, quantity, out api);

                if (api)
                {
                    return Ok("Product Added to Current Sale.");
                }

                return Ok("Error occurred... See Console");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Sale Transaction
        
        [HttpGet("CalculateAmountForSale")]
        public IActionResult CalculateAmountForSale()
        {
            try
            {
                decimal price = -1;
                price = SalesTransaction.CalculateAmountForSale();

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

        [HttpGet("GenerateReceipt")]
        public IActionResult GenerateReceipt()
        {
            try
            {
                string receipt = SalesTransaction.GenerateReceipt();

                if (receipt != "")
                {
                    return Ok(receipt);
                }

                return Ok("Error occurred... See Console");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("TransactSale")]
        public IActionResult TransactSale()
        {
            try
            {
                bool api = true;
                SalesTransaction.TransactSale(out api);

                if(api)
                {
                    return Ok("Current Sale Transaction done.");
                }

                return Ok("Error occurred... See Console");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #endregion
    }
}
