using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_ITS.MODEL.Entities
{
    public class User
    {
        [JsonProperty("id")]
        [Required]
        [RegularExpression("^user_\\d+$")]
        public string Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression("Admin|Cashier", ErrorMessage = "The role must be either 'Admin' or 'Cashier'.")]
        public string Role { get; set; } = string.Empty;
    }
}
