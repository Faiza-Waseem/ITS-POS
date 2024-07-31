using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_ITS.SERVICE.InventoryService
{
    public interface IInventoryService
    {
        Task<int> TrackProductQuantityAsync(int id);
        Task IncreaseProductQuantityAsync(int id, int quantity);
        Task<decimal> GetProductPriceAsync(int id);
        Task ChangeProductPriceAsync(int id, decimal price);
    }
}
