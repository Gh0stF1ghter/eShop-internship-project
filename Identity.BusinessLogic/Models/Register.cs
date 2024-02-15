using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Identity.Core.Models
{
    public class Register
    {
        [Required(ErrorMessage = "Username field should not be empty")]
        [MaxLength(20, ErrorMessage = "Username cannot be longer than 20 characters")]
        [DisplayName("Username")]
        public required string Username { get; init; }

        [Required(ErrorMessage = "Email field should not be empty")]
        [EmailAddress]
        [DisplayName("Email")]
        public required string Email { get; init; }

        [Required(ErrorMessage = "Password field should not be empty")]
        [DataType(DataType.Password)]
        [StringLength(100 , MinimumLength = 6, ErrorMessage = "Password must be longer than 6 characters")]
        [RegularExpression("^.*(?=.{8,})(?=.*[a-zA-Z])(?=.*\\d)(?=.*[!#$%&? ]).*$", ErrorMessage = "Password must have at least one lowercase char, uppercase, digit and special character")]
        [DisplayName("Password")]
        public required string Password { get; init; }
        
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password does not match")]
        [DisplayName("Confirm Password")]
        public required string ConfirmPassword { get; init; }
    }
}
