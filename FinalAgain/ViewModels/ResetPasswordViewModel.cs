using System.ComponentModel.DataAnnotations;

namespace FinalAgain.ViewModels
{
    public class ResetPasswordViewModel
    {
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
