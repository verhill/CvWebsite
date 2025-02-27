using CV_Website.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CV_Website.ViewModels
{
    public class UserSettingsViewModel
        {
            public int Id { get; set; }

            [StringLength(50, MinimumLength = 2, ErrorMessage = "Namnet måste vara minst 2 tecken långt & får inte överskrida 50 bokstäver")]   
            [RegularExpression("^[A-Za-z]+$", ErrorMessage = "Hörru, ditt namn består av bokstäver inget annat")]
            public string Name { get; set; }

            [StringLength(50, ErrorMessage = "Ange en giltig adress")]
            [RegularExpression(@"^[A-Za-z0-9\s]+$", ErrorMessage = "Bokstäver och siffror är tillåtet, inget annat")]   
            public string Address { get; set; }

            [EmailAddress(ErrorMessage = "Ange en giltig e-postadress.")]
            public string UserName { get; set; }

        //Får in att ett telefonnummer inte vår vara mer än 7 tecken efter -46 eller 07+valfri siffra, funkar bra för svenska nummer, kommer inte kunna hantera andra
            [RegularExpression(@"^(\+46|0)(7[0-9]{1})[0-9]{7}$", ErrorMessage = "Ange ett giltigt telefonnummer.")]
            public string PhoneNumber { get; set; }
            public bool Private { get; set; }

            public string Email { get; set; }
        }
}
