using Microsoft.AspNetCore.Mvc;
using AttendanceAppProject.ApiService.Data.Models;
using AttendanceAppProject.ApiService.Services;
using AttendanceAppProject.Dto.Models;
using Microsoft.EntityFrameworkCore;

// API Controller for Quiz Questions

namespace AttendanceAppProject.ApiService.Controllers
{
    [Route("api/[controller]")] // Automatically becomes "api/QuizQuestion"
    [ApiController]
    public class QuizQuestionController : ControllerBase
    {
        private readonly QuizQuestionService _service;

        // Constructor to initialize the QuizQuestionService
        public QuizQuestionController(QuizQuestionService service)
        {
            _service = service;
        }

        /* GET: api/quizquestions/{QuizId}
		 * Get all quiz questions for a given quiz ID
		 * - request body: none
		 * - response body: IEnumerable<QuizQuestion>
		 */
        [HttpGet("{QuizId}")]
        public async Task<ActionResult<IEnumerable<QuizQuestion>>> GetQuestionsById(Guid QuizId)
        {
            // Log the request to retrieve quiz questions for the given QuizId
            return Ok(await _service.GetQuestionsByIdAsync(QuizId));
        }

        /* POST: api/QuizQuestion
		 * Add a new quiz question to the database
		 * - request body: QuizQuestionDto
		 * - response body: QuizQuestion
		 */
        [HttpPost]
        public async Task<ActionResult<QuizQuestion>> AddQuizQuestion([FromBody] QuizQuestionDto dto)
        {
            var newQuestion = await _service.AddQuizQuestionAsync(dto);

            // Return the created quiz question with a 201 status code
            return CreatedAtAction(nameof(GetQuestionsById), new { QuizId = newQuestion.QuizId }, newQuestion);
        }

        /* DELETE: api/QuizQuestion/{QuestionId}
		 * Delete a quiz question from the database
		 * - request body: none
		 * - response body: Status of the deletion
		 */
        [HttpDelete("{QuestionId}")]
        public async Task<ActionResult> DeleteQuizQuestion(Guid QuestionId)
        {
            var result = await _service.DeleteQuizQuestionAsync(QuestionId);

            // If the question wasn't found, return NotFound
            if (!result)
            {
                return NotFound();
            }

            // Return status of deletion
            return NoContent();
        }
    }
}