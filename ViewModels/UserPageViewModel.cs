using CV_Website.Models;

namespace CV_Website.ViewModels
{
    public class UserPageViewModel
    {
        public List<Project> Projects { get; set; }
        public CV UserCV { get; set; }
        public User User { get; set; }
        public List<Skills> Skills { get; set; }
        public List<Experience> Experiences { get; set; }
        public List<Education> Educations { get; set; }
    }
}
