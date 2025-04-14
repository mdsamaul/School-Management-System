using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSets
        public DbSet<RefreshToken> refreshTokens { get; set; }
        public DbSet<Assignments> assignments { get; set; }
        public DbSet<AssignmentSubmissions> assignmentSubmissions { get; set; }
        public DbSet<Classes> classes { get; set; }
        public DbSet<ClassSchedule> classSchedules { get; set; }
        public DbSet<LoginHistory> loginHistories { get; set; }
        public DbSet<Staff> staff { get; set; }
        public DbSet<StudentHistory> studentHistories { get; set; }
        public DbSet<Students> students { get; set; }
        public DbSet<Subjects> subjects { get; set; }
        public DbSet<TeacherClasses> teacherClasses { get; set; }
        public DbSet<Teachers> teachers { get; set; }
        public DbSet<Admins> admins { get; set; }
        public DbSet<Schools> schools { get; set; }
        public DbSet<StudentMarks> studentMarks { get; set; }
        public DbSet<Attendance> attendances { get; set; }
        public DbSet<StudentClasses> studentClasses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Attendance → Students
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Students)
                .WithMany(s => s.attendances)
                .HasForeignKey(a => a.SId)
                .OnDelete(DeleteBehavior.Cascade);

            // Attendance → Classes
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Classes)
                .WithMany(c => c.attendances)
                .HasForeignKey(a => a.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            // StudentClasses → Students
            modelBuilder.Entity<StudentClasses>()
                .HasOne(sc => sc.Students)
                .WithMany(s => s.studentClasses)
                .HasForeignKey(sc => sc.SId)
                .OnDelete(DeleteBehavior.Cascade);

            // StudentClasses → Classes
            modelBuilder.Entity<StudentClasses>()
                .HasOne(sc => sc.Classes)
                .WithMany(c => c.studentClasses)
                .HasForeignKey(sc => sc.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            // StudentMarks → Students
            modelBuilder.Entity<StudentMarks>()
                .HasOne(sm => sm.Students)
                .WithMany(s => s.studentMarks)
                .HasForeignKey(sm => sm.SId)
                .OnDelete(DeleteBehavior.Cascade);

            // StudentMarks → Subjects
            modelBuilder.Entity<StudentMarks>()
                .HasOne(sm => sm.Subjects)
                .WithMany(su => su.studentMarks)
                .HasForeignKey(sm => sm.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // Assignments → Classes
            modelBuilder.Entity<Assignments>()
                .HasOne(a => a.Classes)
                .WithMany()
                .HasForeignKey(a => a.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

            // Assignments → Subjects
            modelBuilder.Entity<Assignments>()
                .HasOne(a => a.Subjects)
                .WithMany()
                .HasForeignKey(a => a.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // Assignments → Teachers
            modelBuilder.Entity<Assignments>()
                .HasOne(a => a.Teachers)
                .WithMany()
                .HasForeignKey(a => a.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            // ClassSchedule Configuration
            modelBuilder.Entity<ClassSchedule>()
                .HasOne(cs => cs.Teachers)
                .WithMany(t => t.classSchedule)
                .HasForeignKey(cs => cs.TeacherId)
                .OnDelete(DeleteBehavior.Cascade); // Use DeleteBehavior as per your requirements

            modelBuilder.Entity<ClassSchedule>()
                .HasOne(cs => cs.Classes)
                .WithMany(c => c.classSchedule)
                .HasForeignKey(cs => cs.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClassSchedule>()
                .HasOne(cs => cs.Subjects)
                .WithMany(s => s.classSchedule)
                .HasForeignKey(cs => cs.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClassSchedule>()
                .HasOne(cs => cs.Schools)
                .WithMany(s => s.ClassSchedules)
                .HasForeignKey(cs => cs.SchoolId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ClassSchedule>()
       .HasOne(cs => cs.Classes)
       .WithMany(c => c.classSchedule)
       .HasForeignKey(cs => cs.ClassId)
       .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClassSchedule>()
                .HasOne(cs => cs.Schools)
                .WithMany(s => s.ClassSchedules)
                .HasForeignKey(cs => cs.SchoolId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClassSchedule>()
                .HasOne(cs => cs.Subjects)
                .WithMany(s => s.classSchedule)
                .HasForeignKey(cs => cs.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClassSchedule>()
                .HasOne(cs => cs.Teachers)
                .WithMany(t => t.classSchedule)
                .HasForeignKey(cs => cs.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TeacherClasses>(entity =>
            {
                entity.HasKey(tc => tc.TeacherClasseId);

                entity.HasOne(tc => tc.Teachers)
                      .WithMany()
                      .HasForeignKey(tc => tc.TeacherId)
                      .OnDelete(DeleteBehavior.Restrict); // Cascading বন্ধ

                entity.HasOne(tc => tc.Classes)
                      .WithMany()
                      .HasForeignKey(tc => tc.ClassId)
                      .OnDelete(DeleteBehavior.Restrict); // Cascading বন্ধ

                entity.HasOne(tc => tc.Subjects)
                      .WithMany()
                      .HasForeignKey(tc => tc.SubjectId)
                      .OnDelete(DeleteBehavior.Restrict); // Cascading বন্ধ
            });
            modelBuilder.Entity<AssignmentSubmissions>(entity =>
            {
                entity.HasKey(e => e.AssignmentSubmissionId);

                entity.HasOne(e => e.Assignments)
                      .WithMany()
                      .HasForeignKey(e => e.AssignmentId)
                      .OnDelete(DeleteBehavior.Cascade); // Assignment ডিলিট হলে submission ডিলিট হবে

                entity.HasOne(e => e.Students)
                      .WithMany()
                      .HasForeignKey(e => e.SId)
                      .OnDelete(DeleteBehavior.Restrict); // Student ডিলিট করলে Submission রুখে দেবে
            });
        }
    }

    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
