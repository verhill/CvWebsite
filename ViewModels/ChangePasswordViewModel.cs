using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CV_Website.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Nuvarande lösenord")]
        public string CurrentPassword { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "Lösenordet måste vara minst 6 tecken långt.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*\W).+$",
        ErrorMessage = "Lösenordet måste innehålla minst en versal, en siffra och ett specialtecken.")]
        [DataType(DataType.Password)]
        [DisplayName("Nytt lösenord")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Matchar ej ditt nya lösenord.")]
        [DisplayName("Bekräfta lösenord")]
        public string ConfirmPassword { get; set; }
    }
}
