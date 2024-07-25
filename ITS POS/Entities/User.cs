using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ITS_POS.Entities
{
    public class User
    {
        private readonly int userId;

        public User() { }

        public int UserId { get { return this.userId; } }

        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role {  get; set; }

        public override string ToString()
        {
            return $"User ID: {UserId}, User Name: {Username}, User Email: {Email}, User Role: {Role}";
        }
    }
}