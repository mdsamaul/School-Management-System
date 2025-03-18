using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.DTO;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace SchoolManagementSystem.Controllers
{
    [Authorize(Roles = "Principal,computerOperator")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly RoleIdService _roleIdService;
        private readonly AppDbContext _context;

        public UserController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            RoleIdService roleIdService,
            AppDbContext context
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleIdService = roleIdService;
            _context = context;
        }
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CommonDto CommonDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(CommonDto.Role == "Admin" || CommonDto.Role=="SuperAdmin"|| CommonDto.Role== "Principal")
            {
                return BadRequest("Admin connot create Admin , SuperAdmin roles and Principal");
            }
            var roleExits =await _roleManager.RoleExistsAsync(CommonDto.Role);
            if(!roleExits)
            {
                return NotFound("Role Not Found");
                //await _roleManager.CreateAsync(new IdentityRole(CommonDto.Role));
            }
            var user = new IdentityUser { UserName = CommonDto.UserName, Email = CommonDto.Email };
            var result = await _userManager.CreateAsync(user, CommonDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            await _userManager.AddToRoleAsync(user, CommonDto.Role);
            if (CommonDto.Role == "Teacher")
            {
                var teacher = new Teachers()
                {
                    UserId = user.Id,
                    CreatedUserId = userId,
                    UserName = CommonDto.UserName,
                    Email = CommonDto.Email,
                    RoleId = await _roleIdService.GetRoleIdAsync(CommonDto.Role),
                    Department = CommonDto.Department,
                    CreatedAt = DateTime.Now.ToLocalTime()
                };
                await _context.teachers.AddAsync(teacher);
                await _context.SaveChangesAsync(); 
                return Ok("Teacher Create Successfully.");
            }else if (CommonDto.Role == "Staff")
            {
                var staff = new Staff()
                {
                    UserId = user.Id,
                    CreatedUserId = userId,
                    UserName = CommonDto.UserName,
                    Email = CommonDto.Email,
                    DesignationName = null,
                    RoleId = await _roleIdService.GetRoleIdAsync(CommonDto.Role),
                    CreatedAt = DateTime.Now.ToLocalTime()
                };
                await _context.staff.AddAsync(staff);
                await _context.SaveChangesAsync();
                return Ok("Staff Create Successfully.");
            }
            else if (CommonDto.Role == "Student")
            {
                var lastStudent = await _context.students
       .OrderByDescending(s => s.StudentId)
       .FirstOrDefaultAsync();               
                var lastYear = DateTime.UtcNow.Year.ToString();
                string newStudentId;
                if (lastStudent != null && lastStudent.StudentId.StartsWith(lastYear))
                {
                    var lastIdNumeric = int.Parse(lastStudent.StudentId.Substring(4));
                    var newIdNumeric = lastIdNumeric + 1;
                    newStudentId = lastYear + newIdNumeric.ToString("D6");
                }
                else
                {
                    newStudentId = lastYear + "001001";
                }
                var student = new Students()
                {
                    StudentId = newStudentId,
                    ClassRoll = 1,
                    UserId = user.Id,
                    CreatedUserId = userId,
                    UserName = CommonDto.UserName,
                    Email = CommonDto.Email,
                    RoleId = await _roleIdService.GetRoleIdAsync(CommonDto.Role),
                    CreatedAt = DateTime.Now.ToLocalTime(),
                    AdmissionYear = DateTime.Now.Year,
                };
                await _context.students.AddAsync(student);
                await _context.SaveChangesAsync();
                return Ok("Student Create Successfully.");
            }
            else
            {
                return BadRequest("Invalid User");
            }           
        }
    }
}
