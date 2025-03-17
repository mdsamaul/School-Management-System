using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models
{
    public class Department
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public virtual ICollection<Teachers>? Teachers { get; set; }
        public virtual ICollection<Students>? Students { get; set; }
        public virtual ICollection<Courses>? Courses { get; set; }
        public virtual ICollection<Attendance>? Attendances { get; set; }



    }
}
