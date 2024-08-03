using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_ITS.MODEL.Entities
{
    public class SaleProduct
    {
        [JsonProperty("id")]
        [Required]
        [RegularExpression("^sale_product_\\d+$")]
        public string Id { get; set; }
        public int SaleProductId { get; set; }
        public int SaleId { get; set; }
        //public Sale Sale { get; set; } = null;
        public int ProductId { get; set; }
        //public Product Product { get; set; } = null;
        public int Quantity { get; set; }
    }
}
