using System.ComponentModel.DataAnnotations;

namespace CV_Website.ViewModels
{
    public class EditProjectViewModel
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
    }

}
