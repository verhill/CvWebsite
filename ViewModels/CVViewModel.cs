using CV_Website.Models;
using System.ComponentModel.DataAnnotations;

namespace CV_Website.ViewModels
{
    public class CVViewModel
    {
        public int UserId { get; set; }
        public List<Skills> AllSkills { get; set; } = new List<Skills>();
        public List<Education> AllEducations { get; set; } = new List<Education>();
        public List<Experience> AllExperiences { get; set; } = new List<Experience>();
        public CV? UserCV { get; set; }

        // Användarens nuvarande val 
        public List<int> SelectedSkills { get; set; } = new List<int>();  
        public List<int> SelectedEducations { get; set; } = new List<int>();  
        public List<int> SelectedExperiences { get; set; } = new List<int>();


        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Använd bara bokstäver")]
        public string? NewSkill { get; set; }

        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Använd bara bokstäver")]
        public string? NewEducation { get; set; }

        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Använd bara bokstäver")]
        public string? NewExperience { get; set; }
    }
}
