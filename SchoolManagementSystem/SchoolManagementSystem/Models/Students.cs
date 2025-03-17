using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models
{
    public class Students: Users
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SId { get; set; }
        public string StudentId { get; set; }
        //[ForeignKey("Department")]
        //public int DepartmentId { get; set; }
        //public virtual Department Department { get; set; }
        //[ForeignKey("Designation")]
        //public int DesignationId { get; set; }
        //public virtual Designation Designation { get; set; }
        //[ForeignKey("Users")]
        //public string UserId { get; set; } = string.Empty;
        //public virtual Users Users { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public int AdmissionYear { get; set; }
        //public virtual ICollection<StudentHistory> StudentHistories { get; set; }
        public virtual ICollection<AssignmentSubmissions> assignmentSubmissions { get; set; }
        public virtual ICollection<Attendance> attendances { get; set; }
        public virtual ICollection<StudentClasses> studentClasses { get; set; }
        public virtual ICollection<StudentHistory> studentHistory { get; set; }
        public virtual ICollection<StudentMarks> studentMarks { get; set; }

    }
}
