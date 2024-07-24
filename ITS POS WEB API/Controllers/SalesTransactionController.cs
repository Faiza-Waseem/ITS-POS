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
        #region Data Members

        private readonly ISalesTransaction __salesTransaction;

        #endregion

        #region Constructor

        public SalesTransactionController(ISalesTransaction salesTransaction)
        {
            __salesTransaction = salesTransaction;
        }

        #endregion

        #region Functions

        #region Product Addition to Sale

        [HttpPost("AddProductToSale")]
        public IActionResult AddProductToSale([FromQuery] String productName, [FromQuery] int quantity)
        {
            try
            {
                bool api = true;
                __salesTransaction.AddProductToSale(productName, quantity, out api);

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
                price = __salesTransaction.CalculateAmountForSale();

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
                string receipt = __salesTransaction.GenerateReceipt();

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
                __salesTransaction.TransactSale(out api);

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
