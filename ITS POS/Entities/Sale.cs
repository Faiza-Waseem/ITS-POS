using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS_POS.Entities
{
    public class Sale
    {
        public int SaleId { get; set; }
        public List<Product> SaleProducts { get; set; } = new List<Product>();
    }
}
