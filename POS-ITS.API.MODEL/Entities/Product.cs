using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_ITS.MODEL.Entities
{
    public class Product
    {
        [JsonProperty("id")]
        [Required]
        [RegularExpression("^product_\\d+$")]
        public string Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ProductType { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ProductCategory { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue)]
        public int ProductQuantity { get; set; }

        [Required]
        [Range(1, (double)decimal.MaxValue)]
        public decimal ProductPrice { get; set; }
    }
}
