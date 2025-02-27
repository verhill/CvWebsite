using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CV_Website.Models
{
    public class CV
    {
        public int CVId { get; set; }
        public int UserId{ get; set; }
        public int ViewCount { get; set; } = 0;
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        public virtual ICollection<Skills> Skills { get; set; } = new List<Skills>();
        public virtual ICollection<Education> Education { get; set; } = new List<Education>();
        public virtual ICollection<Experience> Experience { get; set; } = new List<Experience>();
    }
}
