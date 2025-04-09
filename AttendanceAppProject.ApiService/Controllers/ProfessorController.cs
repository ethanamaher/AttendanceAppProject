/* Professor API Controller
 * Handles HTTP GET and POST requests for attendance instances, allowing for retrieval and creation of professors.
 * Written by Maaz Raza 
 */

using Microsoft.AspNetCore.Mvc;
using AttendanceAppProject.ApiService.Data.Models;
using Microsoft.EntityFrameworkCore;
using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.Dto.Models;

// API controller for Professor

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

        /* GET: api/professor
         * Get all professor records
         * - request body: none
         * - response body: Professors
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Professor>>> GetProfessors()
        {
            return await _context.Professors.ToListAsync();
        }

        /* POST: api/Professor
         * Add a professor to the database
         * - request body: ProfessorDto
         * - response body: Professor
         */
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
