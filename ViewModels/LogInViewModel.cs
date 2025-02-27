
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CV_Website.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vänligen skriv en UserName.")]
        [StringLength(255)]
        [DisplayName("Email")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Vänligen skriv ett lösenord.")]
        [DataType(DataType.Password)]
        [DisplayName("Lösenord")]
        public string Password { get; set; }
        [DisplayName("Kom ihåg mig")]
        public bool RememberMe { get; set; }
    }
}




