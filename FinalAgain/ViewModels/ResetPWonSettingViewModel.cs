using System.ComponentModel.DataAnnotations;

namespace FinalAgain.ViewModels
{
    public class ResetPWonSettingViewModel
    {
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
