using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITS_POS.Entities;

namespace ITS_POS.Data
{
    public static class DataContext
    {
        public static List<User> Users = new List<User>();
        public static List<Product> Inventory = new List<Product>();
        public static List<Sale> Sales = new List<Sale>();

        public static void DisplayUsers()
        {
            foreach (var user in Users)
            {
                Console.WriteLine(user);
            }
        }

        public static void DisplayInventory()
        {
            foreach(var product in Inventory)
            {
                Console.WriteLine(product);
            }
        }

        public static void DisplaySale()
        {
            foreach(var sale in Sales)
            {
                Console.WriteLine(sale); 
            }
        }
    }
}
