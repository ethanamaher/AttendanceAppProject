using Microsoft.AspNetCore.Mvc;
using AttendanceAppProject.ApiService.Data.Models;
using Microsoft.EntityFrameworkCore;
using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.Dto.Models;

namespace AttendanceAppProject.ApiService.Controllers
{
    [Route("api/[controller]")] // Automatically becomes "api/professor"
    [ApiController]
    public class ProfessorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProfessorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Professor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Professor>>> GetProfessors()
        {
            return await _context.Professors.ToListAsync();
        }

        // POST: api/Professor
        [HttpPost]
        public async Task<ActionResult<Student>> AddProfessor([FromBody] ProfessorDto dto)
        {
            var professor = new Professor
            {
                UtdId = dto.UtdId,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            _context.Professors.Add(professor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProfessors), new { id = professor.UtdId }, professor);
        }

    }
}
