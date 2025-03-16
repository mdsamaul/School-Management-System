using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.DTO;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Services;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SchoolManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly TokenService _tokenService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public AuthController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            TokenService tokenService,
            RoleManager<IdentityRole> roleManager,
            AppDbContext context
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
            _context = context;
        }
        [HttpPost("RegisterSuperAdmin")]
        public async Task<IActionResult> RegisterSuperAdmin([FromBody] RegisterDto registerDto)
        {
            if(await _userManager.GetUsersInRoleAsync("SuperAdmin") != null && (await _userManager.GetUsersInRoleAsync("SuperAdmin")).Count > 0)
            {
                return BadRequest("SuperAdmin Already exists");
            }
            if (!await _roleManager.RoleExistsAsync("SuperAdmin"))
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            if (!await _roleManager.RoleExistsAsync("User"))
                await _roleManager.CreateAsync(new IdentityRole("User"));
            var user = new IdentityUser { UserName = registerDto.Email, Email = registerDto.Email };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            await _userManager.AddToRoleAsync(user, "SuperAdmin");
            return Ok("Supper Admin created successfully.");
        }
        //register admin (only super admin can create admin)
        [Authorize(Roles="SuperAdmin")]
        [HttpPost("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDto registerDto)
        {
            var user =new IdentityUser { UserName = registerDto.Email, Email = registerDto.Email };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            await _userManager.AddToRoleAsync(user, "Admin");
            return Ok("Admin Created Successfully.");
        }
        //login any user
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if(user== null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return Unauthorized("Invalid credentials");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.GenerateToken(user, roles);
            //login histomry 
            var loginHostory = new LoginHistory()
            {
                UserId = user.Id,
                LoginTime = DateTime.UtcNow,
                LastActivityTime = DateTime.UtcNow,
                LogoutTime = null
            };
           await _context.loginHistories.AddAsync(loginHostory);
            await _context.SaveChangesAsync();
            return Ok(new { Token = token });
        }
        [Authorize]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId != null)
            {
                var loginEntity = await _context.loginHistories
                    .Where(l => l.UserId == userId && l.LogoutTime == null)
                    .OrderByDescending(l => l.LoginTime)
                    .FirstOrDefaultAsync();
                if(loginEntity != null)
                {
                    loginEntity.LogoutTime = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
            }
            return Ok("User Logged out successfully.");
        }
        [Authorize]
        [HttpPost("UpdateActivity")]
        public async Task<IActionResult> UpdateActivity()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId != null)
            {
                var loginEntry = await _context.loginHistories
                    .Where(l => l.UserId == userId && l.LogoutTime == null)
                    .OrderByDescending(l => l.LoginTime).FirstOrDefaultAsync();
            if(loginEntry != null)
                {
                    loginEntry.LastActivityTime = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
            }
            return Ok("Active Update.");
        }
        [HttpGet("ActiveUsers")]
        public async Task<IActionResult> GetActiveUsers()
        {
            var activeUsers = await _context.loginHistories
                .Where(l => l.LogoutTime == null)
                .Select(l => l.UserId)
                .Distinct().ToListAsync();
            return Ok(new { count= activeUsers.Count, Users=activeUsers});
        }

        //todo call fontend after 30 secend 
        //setInterval(function () {
        //    fetch('/api/Auth/UpdateActivity', {
        //    method: 'POST',
        //headers: { 'Authorization': 'Bearer ' + localStorage.getItem('token') }
        //    });
        //}, 30000); 
    }
}
