/* Class API Controller
 * Handles HTTP GET and POST requests for classes, allowing for retrieval and creation of class records, retrieving a class by its ID, and checking if a class exists in the database.
 * Written by Ethan Maher, Maaz Raza
 */

using Microsoft.AspNetCore.Mvc;
using AttendanceAppProject.ApiService.Services;
using AttendanceAppProject.Dto.Models;
using AttendanceAppProject.ApiService.Data.Models;

namespace AttendanceAppProject.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly ClassService _service;

        // Dependency injection of the ClassService
        public ClassController(ClassService service)
        {
            _service = service;
        }

        /* GET: api/Class
         * Get all classes
         * - request body: none
         * - response body: Classes
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Class>>> GetClasses()
        {
            return Ok(await _service.GetAllClassesAsync());
        }

        /* GET: api/Class/{id}
         * Get class whose classId primary key = id
         * - request body: Guid classId
         * - response body: Class
         */
        [HttpGet("{id}")]
        public async Task<ActionResult<Class>> GetClass(Guid id)
        {
            var result = await _service.GetClassByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /* GET: api/Class/professor/{profUtdId}
         * Get all classes for a particular professor
         * - request body: none
         * - response body: List of Classes
         */
        [HttpGet("professor/{profUtdId}")]
        public async Task<ActionResult<IEnumerable<Class>>> GetClassesByProfessorId(string profUtdId)
        {
            var result = await _service.GetClassesByProfessorIdAsync(profUtdId);
            if (result == null || !result.Any())
                return NotFound();

            return Ok(result);
        }

        /* POST: api/Class
         * Add a class to the database
         * - request body: ClassDto
         * - response body: Class
         */
        [HttpPost]
        public async Task<ActionResult<Class>> AddClass([FromBody] ClassDto dto)
        {
            var newClass = await _service.AddClassAsync(dto);
            return CreatedAtAction(nameof(GetClass), new { id = newClass.ClassId }, newClass);
        }

        /* POST: api/Class/exists
         * Check if a class exists in the database by validating the classId passed in by the client side
         * - request body: classId
         * - response body: HttpResponse
         */
        [HttpPost("exists")]
        public async Task<ActionResult<bool>> ClassExists([FromBody] Guid classId)
        {
            if (string.IsNullOrWhiteSpace(classId.ToString()))
                return BadRequest("Class ID is required.");

            var exists = await _service.ClassExistsAsync(classId);
            if (!exists)
                return NotFound($"Class with ID {classId} not found");

            return Ok(true);
        }
    }
}