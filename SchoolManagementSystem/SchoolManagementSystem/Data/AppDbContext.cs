using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<RefreshToken> refreshTokens { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }
        public DbSet<Assignments> assignments { get; set; }
        public DbSet<AssignmentSubmissions> assignmentSubmissions { get; set; }
        public DbSet<Attendance> attendances { get; set; }
        public DbSet<Classes> classes { get; set; }
        public DbSet<ClassSchedule> classSchedules { get; set; }
        public DbSet<LoginHistory> loginHistories { get; set; }
        public DbSet<Staff> staff{ get; set; }
        public DbSet<StudentClasses> studentClasses{ get; set; }
        public DbSet<StudentHistory> studentHistories{ get; set; }
        public DbSet<StudentMarks> studentMarks{ get; set; }
        public DbSet<Students> students{ get; set; }
        public DbSet<Subjects> subjects{ get; set; }
        public DbSet<TeacherClasses> teacherClasses{ get; set; }
        public DbSet<Teachers> teachers{ get; set; }
       

    }
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
    

}
