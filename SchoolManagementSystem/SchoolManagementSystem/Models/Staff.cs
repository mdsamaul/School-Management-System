using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models
{
    public class Staff:Users
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StaffId { get; set; }
        public DateTime JoiningDate { get; set; }
        [ForeignKey("Designation")]
        public int DesignationId { get; set; }
        public virtual Designation Designation { get; set; }

    }
}
