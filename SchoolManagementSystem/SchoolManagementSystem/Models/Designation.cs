using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models
{
    public class Designation
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DesignationId { get; set; }
        public string DesignationName { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public virtual ICollection<Teachers> Teachers { get; set; }
        public virtual ICollection<Students> Students { get; set; }
        public virtual ICollection<Staff> Staffs { get; set; }

    }
}
