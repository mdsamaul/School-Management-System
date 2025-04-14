using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models
{
    public class Assignments
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssignmentId { get; set; }
        public string? Title { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        //[ForeignKey("Courses")]
        //public int CourseId { get; set; }
        //public virtual Courses Courses { get; set; }
        [ForeignKey("Teachers")]
        public int TeacherId { get; set; }
        public virtual Teachers Teachers { get; set; }
        [ForeignKey("Classes")]
        public int ClassId { get; set; }
        public virtual Classes Classes { get; set; }
        [ForeignKey("Subjects")]
        public int SubjectId { get; set; }
        public virtual Subjects Subjects { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public virtual ICollection<AssignmentSubmissions> assignmentSubmissions { get; set; }
    }
}
