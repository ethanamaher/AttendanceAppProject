/* Professor API Controller
 * Handles HTTP GET, POST, PUT, and DELETE requests for professor, allowing for retrieval and creation of professors.
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

        // Dependency injection of the professor service
        public ProfessorController(ProfessorService service)
        {
            _service = service;
        }

        /* GET: api/professor
         * Get all professor records
         * - request body: none
         * - response body: <IEnumerable<Professor>>
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Professor>>> GetProfessors()
        {
            return Ok(await _service.GetProfessorsAsync());
        }


        /* GET: api/Professor/{professorId}
         * Get Professor record for given ProfessorId
         * - request body: profUtdId string
         * - response body: Professor
         */

        [HttpGet("{UtdId}")]
        public async Task<ActionResult<Professor>> GetProfessor(string UtdId)
        {
            var professor = await _service.GetProfessorByIdAsync(UtdId);
            if(professor == null)
            {
                return NotFound();
            }
            return professor;
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

        /* PUT: api/Professor/{utdId}
         * Update a professor by UtdId
         * - request body: ProfessorDto
         * - response body: Professor
         */
        [HttpPut("{utdId}")]
        public async Task<ActionResult<Professor>> UpdateProfessor(string utdId, [FromBody] ProfessorDto updatedProfessor)
        {
            var professor = await _service.UpdateProfessorAsync(utdId, updatedProfessor);
            if (professor == null)
                return NotFound();

            return Ok(professor);
        }

        /* DELETE: api/Professor/{utdId}
         * Delete a professor by UtdId
         * - request body: none
         * - response body: HttpResponse
         */
        [HttpDelete("{utdId}")]
        public async Task<IActionResult> DeleteProfessor(string utdId)
        {
            var success = await _service.DeleteProfessorAsync(utdId);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}