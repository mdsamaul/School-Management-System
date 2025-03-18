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
        private readonly RoleIdService _roleIdService;

        public AuthController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            TokenService tokenService,
            RoleManager<IdentityRole> roleManager,
            AppDbContext context,
            RoleIdService roleIdService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
            _context = context;
            _roleIdService = roleIdService;
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
            var user = new IdentityUser { UserName = registerDto.UserName, Email = registerDto.Email };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            await _userManager.AddToRoleAsync(user, "SuperAdmin");
            return Ok("Supper Admin created successfully.");
        }
        [Authorize(Roles ="SuperAdmin")]
        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody] RoleDto roleDto)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleDto.RoleName);          
            if (!roleExist)
            {
                var result =await _roleManager.CreateAsync(new IdentityRole(roleDto.RoleName));
                if (!result.Succeeded) return BadRequest(result.Errors);
                else return Ok("Role Create Successfully.");
            }
            return BadRequest("Role already exist!");
        }
        [HttpGet("GetAllRole")]
        public async Task<IActionResult> GetAllRole()
        {
            var roles = _roleManager.Roles.ToList();
            if (roles != null)
                return Ok(roles);
            else return BadRequest("Role Not Found");
        }
        //register admin (only super admin can create admin)
        [Authorize(Roles="SuperAdmin")]
        [HttpPost("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDto registerDto)
        {
            var roleExist = await _roleManager.RoleExistsAsync(registerDto.Role);
            if (!roleExist)
            {
                //await _roleManager.CreateAsync(new IdentityRole("Admin"));
                return BadRequest("Role Not Fond");
            }
            var user =new IdentityUser { UserName = registerDto.UserName, Email = registerDto.Email };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            await _userManager.AddToRoleAsync(user, "Admin");
            return Ok("Admin Created Successfully.");
        }//register principal (only admin can create Principal)
        [Authorize(Roles = "Admin")]
        [HttpPost("RegisterPrincipal")]
        public async Task<IActionResult> RegisterPrincipal([FromBody] RegisterDto registerDto)
        {         
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // Check if "Principal" role exists, if not create it
            var roleExist = await _roleManager.RoleExistsAsync(registerDto.Role);
            if (!roleExist)
            {
                return BadRequest("Role Not Found");
                //await _roleManager.CreateAsync(new IdentityRole("Principal"));
            }

            var user = new IdentityUser { UserName = registerDto.UserName, Email = registerDto.Email };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, "Principal");
            if (registerDto.Role == "Principal")
            {
                var teacher = new Teachers()
                {
                    UserId=user.Id,
                    CreatedUserId = userId,
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                    RoleId = await _roleIdService.GetRoleIdAsync(registerDto.Role),
                    Department = registerDto.Department,
                    CreatedAt = DateTime.Now.ToLocalTime()
                };
                await _context.teachers.AddAsync(teacher);
               await _context.SaveChangesAsync();               
            }
            return Ok("Principal Created Successfully");        }

        

        //login any user
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            //var user = await _userManager.FindByEmailAsync(loginDto.Email , loginDto.UserName);
            var user =await _userManager.Users.Where(l => l.Email == loginDto.Email || l.UserName == loginDto.UserName).FirstOrDefaultAsync();
            if (user== null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return Unauthorized("Invalid credentials");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.GenerateToken(user, roles);
            //login histomry 
            var loginHostory = new LoginHistory()
            {
                UserId = user.Id,
                LoginTime = DateTime.UtcNow.ToLocalTime(),
                LastActivityTime = DateTime.UtcNow.ToLocalTime(),
                LogoutTime = null
            };
           await _context.loginHistories.AddAsync(loginHostory);
            await _context.SaveChangesAsync();
            //TimeZoneInfo bangladeshTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Bangladesh Standard Time");
            //DateTime bangladeshLoginTime = TimeZoneInfo.ConvertTimeFromUtc(loginHostory.LoginTime, bangladeshTimeZone);

            //string formattedTime = bangladeshLoginTime.ToString("yyyy-MM-dd HH:mm:ss");

           
            return Ok(new { Token = token});
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
                    loginEntity.LogoutTime = DateTime.UtcNow.ToLocalTime();
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
