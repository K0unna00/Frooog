using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FinalAgain.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password not match.")]
        public string ConfirmPassword { get; set; }
        public string UserPP { get; set; }
        public IFormFile UserPPFile { get; set; }
    }
}
