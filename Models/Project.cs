using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CV_Website.Models
{
    public class Project
    {
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string Description { get; set; }
        [StringLength(1000, ErrorMessage = "Information cannot be longer than 1000 characters")]
        public string? Information { get; set; }
        public int CreatorId { get; set; }
        [ForeignKey(nameof(CreatorId))]
        public virtual User Creator { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();
        
    }
}
