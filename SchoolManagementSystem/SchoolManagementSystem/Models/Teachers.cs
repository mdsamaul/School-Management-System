using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models
{
    public class Teachers:Users
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeacherId { get; set; }
        //[ForeignKey("Department")]
        //public int DepartmentId { get; set; }
        //public virtual Department Department {  get; set; }
        //[ForeignKey("Designation")]
        //public int DesignationId {  get; set; }
        //public virtual Designation Designation { get; set; }
        public string Department { get; set; }
        public virtual ICollection<Assignments> assignments { get; set; }
        public virtual ICollection<ClassSchedule> classSchedule { get; set; }
        public virtual ICollection<TeacherClasses>teacherClasses { get; set; }
    }
}
