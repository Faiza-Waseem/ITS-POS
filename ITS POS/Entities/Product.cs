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
        public ICollection<SaleProduct> Sales { get; } = new List<SaleProduct>();

        public override string ToString()
        {
            return $"Product ID: {ProductId}\nProduct Name: {ProductName}\nProduct Type: {ProductType}\nProduct Category: {ProductCategory}\nProduct Quantity: {ProductQuantity}\nProduct Price:{ProductPrice}";
        }
    }
}
