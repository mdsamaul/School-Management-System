using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Models
{
    public class StudentHistory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentHistoryId { get; set; }
        [ForeignKey("Students")]
        public int SId { get; set; }
        public virtual Students Students { get; set; }       
        public string? Previous { get; set; }
        public string? Status {  get; set; }
        public int?  Year { get; set; }
    }
}
