using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CV_Website.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [DisplayName("Namn")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("Email")]
        public string UserName { get; set; }

        [Required]
        [Phone]
        [DisplayName("Telefon")]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        
        [Required(ErrorMessage = "Lösenord är obligatoriskt.")]
        
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Lösenordet måste vara minst 6 tecken långt.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*\W).+$",
        ErrorMessage = "Lösenordet måste innehålla minst en versal, en siffra och ett specialtecken.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Bekräftelse av lösenord är obligatorisk.")]
        [Compare("Password", ErrorMessage = "Lösenordet och bekräftelselösenordet matchar inte.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DisplayName("Privat")]
        public bool Private { get; set; }
    }

}
