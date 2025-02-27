using System.ComponentModel.DataAnnotations;

namespace CV_Website.Models
{
    public class Experience
    {
        public int ExperienceId { get; set; }
        [RegularExpression("^[A-Za-z]+$", ErrorMessage = "Använd bara bokstäver")]
        public string Name { get; set; }

        public virtual ICollection<CV> CV { get; set; } = new List<CV>();
    }
}
