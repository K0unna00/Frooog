using FinalAgain.Models;

namespace FinalAgain.ViewModels
{
    public class AccountSettingViewModel
    {
        public AppUser user { get; set; }
        public ResetPWonSettingViewModel SecuriryResetPassword { get; set; }
    }
}
