using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<RefreshToken> refreshTokens { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<LoginHistory> loginHistories { get; set; }
        public DbSet<Students> Students { get; set; }
        public DbSet<StudentHistory> StudentsHistory { get; set; }
        public DbSet<Teachers> Teachers { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Attendance> attendances { get; set; }
        public DbSet<Assignments> Assignments { get; set; }
        public DbSet<Courses> Courses { get; set; }
        public DbSet<Department> departments { get; set; }
        public DbSet<Designation> designations { get; set; }

    }
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public DateTime ExpiryDate { get; set; }
    }


}
