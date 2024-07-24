using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS_POS.Services
{
    public interface IInventoryManagement
    {
        #region Functions

        #region Product Tracking

        #region Product Quantity

        public void TrackProductQuantity(string productName, out int quantity);
        public void TrackProductQuantity(string productName);
        public void IncreaseProductQuantity(string productName, int newQuantity, out bool api);
        public void IncreaseProductQuantity(string productName, int newQuantity);

        #endregion

        #region Product Price

        public void CheckProductPrice(string productName, out decimal price);
        public void CheckProductPrice(string productName);
        public void SetProductPrice(string productName, decimal newPrice, out bool api);
        public void SetProductPrice(string productName, decimal newPrice);

        #endregion

        #endregion

        #endregion
    }
}
