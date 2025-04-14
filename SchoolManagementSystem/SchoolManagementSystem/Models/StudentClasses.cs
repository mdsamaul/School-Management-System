using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models
{
    public class StudentClasses
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentClassId { get; set; }
        [ForeignKey("Students")]
        public int SId{ get;set; }
        public virtual Students Students { get; set; }
        [ForeignKey("Classes")]
        public int ClassId { get; set; }
        public virtual Classes Classes { get; set; }
        public int? Year { get; set; }
    }
}
