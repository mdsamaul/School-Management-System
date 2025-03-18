namespace SchoolManagementSystem.Models
{
    public class Users
    {
        public string UserId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; }=string.Empty;
        public string UserName { get; set; }=string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string RoleId { get; set; } = string.Empty;
        public string CreatedUserId { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public bool IsActive { get; set; } 
        public string? Status { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; } = string.Empty;
        public string? Image { get; set; }
    }
}
