using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace OnlineShop.Api.Models
{
    public class SignupModel
    {
        [Required]
        [DisplayName("User name")]
        public string Username { get; set; }

        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        [StringLength(100)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Please confirm password correctly!")]
        public string ConfirmPassword { get; set; }

        public string Email { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(ConfirmPassword) && Password == ConfirmPassword;
        }
    }
}