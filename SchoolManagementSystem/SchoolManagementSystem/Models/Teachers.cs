using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models
{
    public class Teachers:Users
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeacherId { get; set; }       
        public string Department { get; set; }
        public virtual ICollection<Assignments> assignments { get; set; }
        public virtual ICollection<ClassSchedule> classSchedule { get; set; }
        public virtual ICollection<TeacherClasses>teacherClasses { get; set; }
    }
}
