using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ITS_POS.Entities
{
    public class Product
    {
        private readonly int productId;
        public int ProductId { get { return this.productId; } }
        public string ProductName { get; set; }
        public string ProductType { get; set; }
        public string ProductCategory { get; set; }
        public int ProductQuantity { get; set; }
        public decimal ProductPrice { get; set; }
        public ICollection<SaleProduct> Sales = new List<SaleProduct>();

        public override string ToString()
        {
            return $"Product ID: {ProductId}, Product Name: {ProductName}, Product Type: {ProductType}, Product Category: {ProductCategory}, Product Quantity: {ProductQuantity}, Product Price:{ProductPrice}";
        }
    }
}
