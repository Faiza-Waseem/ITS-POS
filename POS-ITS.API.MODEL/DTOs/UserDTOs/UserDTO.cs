using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_ITS.MODEL.DTOs.UserDTOs
{
    public class UserDTO
    {
        [Required(ErrorMessage = "User ID is Required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Product Name is Required.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [PasswordPropertyText]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "User Email is required.")]
        [EmailAddress(ErrorMessage = "Email format must be correct for example: user@example.com")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "User Role is required.")]
        [RegularExpression("Admin|Cashier", ErrorMessage = "The role must be either 'Admin' or 'Cashier'.")]
        public string Role { get; set; } = string.Empty;
    }
}
