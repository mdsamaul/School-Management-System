namespace SchoolManagementSystem.DTO
{
    public class CommonDto
    {

        //registron 
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public string? Department { get; set; }

        //student 
        public int SId { get; set; }
        public string? StudentId { get; set; }
        public int? ClassRoll { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public int? AdmissionYear { get; set; }
        public string? UserId { get; set; } = string.Empty;
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string? RoleId { get; set; } = string.Empty;
        public string? CreatedUserId { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public bool IsActive { get; set; } = false;
        public string? Status { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; } = string.Empty;
        public string? Image { get; set; }
        public IFormFile? ImageFile { get; set; }

        //teacher
        public int? TeacherId { get; set; }
        //staff
        public int? StaffId { get; set; }
        public DateTime? JoiningDate { get; set; }
        public string? DesignationName { get; set; }
    }
}
