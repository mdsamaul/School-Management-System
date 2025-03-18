﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.DTO;
using SchoolManagementSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Controllers
{
    [Authorize(Roles = "Principal,computerOperator")]
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
            if(registerDto.Role == "Admin" || registerDto.Role=="SuperAdmin"|| registerDto.Role== "Principal")
            {
                return BadRequest("Admin connot create Admin , SuperAdmin roles and Principal");
            }
            if(!await _roleManager.RoleExistsAsync(registerDto.Role))
            {
                return NotFound("Role Not Found");
                //await _roleManager.CreateAsync(new IdentityRole(registerDto.Role));
            }
            var user = new IdentityUser { UserName = registerDto.UserName, Email = registerDto.Email };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            await _userManager.AddToRoleAsync(user, registerDto.Role);
            if (registerDto.Role == "Teacher")
            {
                var teacher = new Teachers()
                {
                    //todo
                };
            }
            return Ok("User Create Successfully.");
        }
    }
}
