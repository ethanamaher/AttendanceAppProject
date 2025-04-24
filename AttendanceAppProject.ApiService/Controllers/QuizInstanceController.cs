using Microsoft.AspNetCore.Mvc;
using AttendanceAppProject.ApiService.Data.Models;
using AttendanceAppProject.ApiService.Services;
using AttendanceAppProject.Dto.Models;
using Microsoft.EntityFrameworkCore;

// API Controller for Quiz Instance

namespace AttendanceAppProject.ApiService.Controllers
{
    [Route("api/[controller]")] // Automatically becomes "api/QuizInstance"
    [ApiController]
    public class QuizInstanceController : ControllerBase
    {
        private readonly QuizInstanceService _service;

        // Constructor to initialize the QuizInstanceService
        public QuizInstanceController(QuizInstanceService service)
        {
            _service = service;
        }

        /* GET: api/QuizInstance
		 * Get all quiz instances (quizzes) in the system
		 * - request body: none
		 * - response body: IEnumerable<QuizInstance>
		 */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuizInstance>>> GetQuizzes()
        {
            // Return all quiz instances
            return Ok(await _service.GetQuizzesAsync());
        }

        /* GET: api/QuizInstance/{ClassId}
		 * Get a quiz instance based on the associated class ID
		 * - request body: none
		 * - response body: QuizInstance
		 */
        [HttpGet("{ClassId}")]
        public async Task<ActionResult<QuizInstance>> GetQuizById(Guid ClassId)
        {
            var quizInstance = await _service.GetQuizByIdAsync(ClassId);

            // Check if the quiz instance was found
            if (quizInstance == null)
            {
                return NotFound();
            }

            // Return the found quiz instance
            return Ok(quizInstance);
        }

        /* POST: api/QuizInstance
		 * Add a new quiz instance (quiz) to the database
		 * - request body: QuizInstanceDto
		 * - response body: QuizInstance
		 */
        [HttpPost]
        public async Task<ActionResult<QuizInstance>> AddQuizInstance([FromBody] QuizInstanceDto dto)
        {
            // Add the quiz instance to the database
            var newQuiz = await _service.AddQuizInstanceAsync(dto);

            // Return the created quiz instance with a 201 status code
            return CreatedAtAction(nameof(GetQuizzes), new { id = newQuiz.QuizId }, newQuiz);
        }

        /* PUT: api/QuizInstance/{QuizId}
		 * Update an existing quiz instance's class association
		 * - request body: QuizInstanceDto (classId to update to)
		 * - response body: QuizInstance
		 */
        [HttpPut("{QuizId}")]
        public async Task<ActionResult<QuizInstance>> UpdateQuizClass(Guid QuizId, [FromBody] QuizInstanceDto dto)
        {
            var updatedQuiz = await _service.UpdateQuizClassAsync(QuizId, dto.ClassId);

            // If the quiz wasn't found, return NotFound
            if (updatedQuiz == null)
            {
                return NotFound();
            }

            // Return the updated quiz instance
            return Ok(updatedQuiz);
        }

        /* DELETE: api/QuizInstance/{QuizId}
		 * Delete a quiz instance
		 * - request body: none
		 * - response body: Status of the deletion
		 */
        [HttpDelete("{QuizId}")]
        public async Task<ActionResult> DeleteQuizInstance(Guid QuizId)
        {
            var result = await _service.DeleteQuizInstanceAsync(QuizId);

            // If the quiz wasn't found, return NotFound
            if (!result)
            {
                return NotFound();
            }

            // Return status of deletion
            return NoContent();
        }
    }
}