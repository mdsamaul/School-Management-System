using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models
{
    public class Attendance
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttendanceId { get; set; }
        //[ForeignKey("Courses")]
        //public int CourseId { get; set; }
        //public virtual Courses Courses { get; set; }
        //[ForeignKey("Department")]
        //public int DepartmentId { get; set; }
        //public virtual Department Departments { get; set; }
        public DateTime AttendanceDate {  get; set; }
        public string Status { get; set; }
        public string UserId { get; set; } = string.Empty;
        [ForeignKey("Students")]
        public int SId { get; set; }
        public virtual Students Students { get; set; }
        [ForeignKey("Classes")]
        public int ClassId { get; set; }
        public virtual Classes Classes { get; set; }

    }
}
