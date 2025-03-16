using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.DTO;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Controllers
{
    [Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterDto registerDto)
        {
            if(registerDto.Role == "Admin" || registerDto.Role=="SuperAdmin")
            {
                return BadRequest("Admin connot create Admin or SuperAdmin roles");
            }
            if(!await _roleManager.RoleExistsAsync(registerDto.Role))
            {
                await _roleManager.CreateAsync(new IdentityRole(registerDto.Role));
            }
            var user = new IdentityUser { UserName = registerDto.Email, Email = registerDto.Email };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            await _userManager.AddToRoleAsync(user, registerDto.Role);
            return Ok("User Create Successfully.");
        }
    }
}
