using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.SqlServer.Server;
using Microsoft.AspNetCore.Identity;

namespace CV_Website.Models
{
    public class User : IdentityUser<int>  // Inheriting from IdentityUser with int as the primary key type
    {
        

        [Required(ErrorMessage = "Ange ditt förnamn")]
        [RegularExpression("^[A-Za-z]+$", ErrorMessage = "Hörru, ditt namn består av bokstäver inget annat")]
        public string Name { get; set; }

       

        [StringLength(50, ErrorMessage = "Ange en giltig adress")]
        [RegularExpression(@"^[A-Za-z0-9\s]+$", ErrorMessage = "Bokstäver och siffror är tillåtet, inget annat")]
        public string Address { get; set; }


        public byte[]? img { get; set; }
        public Boolean Private { get; set; }
        public Boolean Deactivated { get; set; }

        public virtual ICollection<CV> CVs { get; set; } = new List<CV>();
        public virtual ICollection<Project> Project { get; set; } = new List<Project>();
        public virtual ICollection<Message> SentMessages { get; set; } = new List<Message>();
        public virtual ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();
    }
}
