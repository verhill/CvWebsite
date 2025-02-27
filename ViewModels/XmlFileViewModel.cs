using CV_Website.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CV_Website.ViewModels
{
    public class XmlFileViewModel 
    {
        
        public string Name { get; set; }
    
        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }
        
        public List<SkillXml> Skills { get; set; } = new List<SkillXml>();
        public List<EducationXml> Educations { get; set; } = new List<EducationXml>();
        public List<ExperienceXml> Experiences { get; set; } = new List<ExperienceXml>();
        public List<ProjectXml> Projects { get; set; } = new List<ProjectXml>();
    }
}
