using Microsoft.AspNetCore.Identity;
using SchoolManagementSystem.Data;

namespace SchoolManagementSystem.Services
{
    public class RoleIdService
    {
        private readonly AppDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleIdService(
            AppDbContext context,
            RoleManager<IdentityRole> roleManager
            )
        {
            _context = context;
            _roleManager = roleManager;
        }
        public async Task<string> GetRoleIdAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            return role?.Id ?? "Role not found";
        }
        
    }
}
