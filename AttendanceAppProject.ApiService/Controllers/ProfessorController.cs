/* Professor API Controller
 * Handles HTTP GET and POST requests for attendance instances, allowing for retrieval and creation of professors.
 * Written by Maaz Raza 
 */
using Microsoft.AspNetCore.Mvc;
using AttendanceAppProject.ApiService.Data.Models;
using Microsoft.EntityFrameworkCore;
using AttendanceAppProject.ApiService.Services;
using AttendanceAppProject.Dto.Models;

namespace AttendanceAppProject.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorController : ControllerBase
    {
        private readonly ProfessorService _service;

        public ProfessorController(ProfessorService service)
        {
            _service = service;
        }

        /* GET: api/professor
         * Get all professor records
         * - request body: none
         * - response body: Professors
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Professor>>> GetProfessors()
        {
            return Ok(await _service.GetProfessorsAsync());
        }

        /* POST: api/Professor
         * Add a professor to the database
         * - request body: ProfessorDto
         * - response body: Professor
         */
        [HttpPost]
        public async Task<ActionResult<Professor>> AddProfessor([FromBody] ProfessorDto dto)
        {
            var professor = await _service.AddProfessorAsync(dto);
            return CreatedAtAction(nameof(GetProfessors), new { id = professor.UtdId }, professor);
        }

        /* POST: api/professor/login
         * Authenticate a professor by ID and password
         * - request body: object with ProfessorId and Password
         * - response body: Professor or 401 Unauthorized
         */
        [HttpPost("login")]
        public async Task<ActionResult<Professor>> Login([FromBody] ProfessorDto dto)
        {
            Console.WriteLine($"Login attempt: {dto.UtdId}");

            var professor = await _service.AuthenticateProfessorAsync(dto.UtdId, dto.Password);

            if (professor == null)
                return Unauthorized("Invalid credentials.");

            return Ok(professor);
        }
    }
}