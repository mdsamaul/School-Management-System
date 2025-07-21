using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models
{
    public class Classes
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        [ForeignKey("Schools")]
        public int SchoolId { get; set; }
        public virtual Schools Schools { get; set; }
        public virtual ICollection<Assignments> assignments { get; set; }
        public virtual ICollection<Attendance> attendances { get; set; }
        public virtual ICollection<ClassSchedule> classSchedule { get; set; }
        public virtual ICollection<StudentClasses> studentClasses { get; set; }
        public virtual ICollection<TeacherClasses> teacherClasses { get; set; }
    }
}
