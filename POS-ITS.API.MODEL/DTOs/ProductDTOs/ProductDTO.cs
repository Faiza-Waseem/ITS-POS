using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_ITS.MODEL.DTOs.ProductDTOs
{
    public class ProductDTO
    {
        [JsonProperty("id")]
        [Required(ErrorMessage = "Id is required.")]
        [RegularExpression("^product_\\d+$", ErrorMessage = "The id must be in the format: \"product_ProductId\".")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Product ID is required.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product Name is Required.")]
        [StringLength(100, ErrorMessage = "Product Name's length cannot be greater than 100.")]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Product Type is Required.")]
        [StringLength(100, ErrorMessage = "Product Type's length cannot be greater than 100.")]
        public string ProductType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Product Category is Required.")]
        [StringLength(100, ErrorMessage = "Product Category's length cannot be greater than 100.")]
        public string ProductCategory { get; set; } = string.Empty;

        [Required(ErrorMessage = "Product Quantity is Required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Product Quantity must be non-negative.")]
        public int ProductQuantity { get; set; }

        [Required(ErrorMessage = "Product Price is Required.")]
        [Range(1, (double)decimal.MaxValue, ErrorMessage = "Product Price must be greater than 0.")]
        public decimal ProductPrice { get; set; }
    }
}
