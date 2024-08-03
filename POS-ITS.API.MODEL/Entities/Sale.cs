using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_ITS.MODEL.Entities
{
    public class Sale
    {
        [JsonProperty("id")]
        [Required]
        [RegularExpression("^sale_product_\\d+$")]
        public string Id { get; set; }
        public int SaleId { get; set; }
        public List<SaleProduct> SaleProducts { get; set; } = new List<SaleProduct>();
    }
}
