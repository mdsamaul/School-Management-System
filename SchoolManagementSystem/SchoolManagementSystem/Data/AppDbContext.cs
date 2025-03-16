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
        public DbSet<LoginHistory> loginHistories { get; set; }
    }
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
    

}
