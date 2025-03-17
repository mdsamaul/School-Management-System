using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models
{
    public class Courses
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        [ForeignKey("Department")]
        public int DeparmentId { get; set; }
        public virtual Department? Department { get; set; }

        public string UserId { get; set; } = string.Empty;
        public virtual ICollection<Assignments> Assignments { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }


    }
}
