using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models
{
    public class AssignmentSubmissions
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssignmentSubmissionId { get; set; }
        [ForeignKey("Assignments")]
        public int AssignmentId { get; set; }
        public virtual Assignments Assignments { get; set; }
        [ForeignKey("Students")]
        public int SId { get; set; }
        public virtual Students Students { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public float? Marks { get; set; }
    }
}
