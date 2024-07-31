using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_ITS.MODEL
{
    public class SaleProduct
    {
        public int SaleProductId { get; }
        public int SaleId { get; set; }
        //public Sale Sale { get; set; } = null;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null;
        public int Quantity { get; set; }
    }
}
