using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models
{
    public class StudentMarks
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MarkId { get; set; }
        [ForeignKey("Students")]
        public int SId { get; set; }
        public virtual Students Students { get; set; }
        [ForeignKey("Subjects")]
        public int SubjectId { get; set; }
        public virtual Subjects Subjects { get; set; }
        public int Year { get; set; }
        public float MarksObtained { get; set; }
    }
}
