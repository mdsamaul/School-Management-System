using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TeachersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Teachers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teachers>>> Getteachers()
        {
            return await _context.teachers.ToListAsync();
        }

        // GET: api/Teachers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Teachers>> GetTeachers(int id)
        {
            var teachers = await _context.teachers.FindAsync(id);

            if (teachers == null)
            {
                return NotFound();
            }

            return teachers;
        }

        // PUT: api/Teachers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeachers(int id, Teachers teachers)
        {
            if (id != teachers.TeacherId)
            {
                return BadRequest();
            }

            _context.Entry(teachers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeachersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Teachers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Teachers>> PostTeachers(Teachers teachers)
        {
            _context.teachers.Add(teachers);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTeachers", new { id = teachers.TeacherId }, teachers);
        }

        // DELETE: api/Teachers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeachers(int id)
        {
            var teachers = await _context.teachers.FindAsync(id);
            if (teachers == null)
            {
                return NotFound();
            }

            _context.teachers.Remove(teachers);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TeachersExists(int id)
        {
            return _context.teachers.Any(e => e.TeacherId == id);
        }
    }
}
