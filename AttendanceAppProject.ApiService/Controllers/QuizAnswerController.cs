using Microsoft.AspNetCore.Mvc;
using AttendanceAppProject.ApiService.Data.Models;
using AttendanceAppProject.ApiService.Services;
using Microsoft.EntityFrameworkCore;
using AttendanceAppProject.Dto.Models;

// API Controller for Quiz Answers

namespace AttendanceAppProject.ApiService.Controllers
{
    [Route("api/[controller]")] // Automatically becomes "api/QuizAnswer"
    [ApiController]
    public class QuizAnswerController : ControllerBase
    {
        private readonly QuizAnswerService _service;

        // Constructor to initialize the QuizAnswerService
        public QuizAnswerController(QuizAnswerService service)
        {
            _service = service;
        }

        /* GET: api/QuizAnswer/{QuestionId}
		 * Get quiz answers by question ID
		 * - request body: none
		 * - response body: IEnumerable<QuizAnswer>
		 */
        [HttpGet("{QuestionId}")]
        public async Task<ActionResult<IEnumerable<QuizAnswer>>> GetAnswersById(Guid QuestionId)
        {
            // Return quiz answers for the given QuestionId
            return Ok(await _service.GetAnswersByIdAsync(QuestionId));
        }

        /* POST: api/QuizAnswer
		 * Add a new quiz answer to the database
		 * - request body: QuizAnswerDto
		 * - response body: QuizAnswer
		 */
        [HttpPost]
        public async Task<ActionResult<QuizAnswer>> AddQuizAnswer([FromBody] QuizAnswerDto dto)
        {
            var newAnswer = await _service.AddQuizAnswerAsync(dto);

            // Return the created quiz answer with a 201 status code
            return CreatedAtAction(nameof(GetAnswersById), new { QuestionId = newAnswer.QuestionId }, newAnswer);
        }

        /* PUT: api/QuizAnswer/{answerId}
         * Update a quiz answer by AnswerId
         * - request body: QuizAnswerDto
         * - response body: QuizAnswer
         */
        [HttpPut("{answerId}")]
        public async Task<ActionResult<QuizAnswer>> UpdateQuizAnswer(int answerId, [FromBody] QuizAnswerDto updatedAnswer)
        {
            var answer = await _service.UpdateQuizAnswerAsync(answerId, updatedAnswer);
            if (answer == null)
                return NotFound();

            return Ok(answer);
        }

        /* DELETE: api/QuizAnswer/{AnswerId}
		 * Delete a quiz answer from the database
		 * - request body: none
		 * - response body: Status of the deletion
		 */
        [HttpDelete("{AnswerId}")]
        public async Task<ActionResult> DeleteQuizAnswer(int AnswerId)
        {
            var result = await _service.DeleteQuizAnswerAsync(AnswerId);

            // If the answer wasn't found, return NotFound
            if (!result)
            {
                return NotFound();
            }

            // Return status of deletion
            return NoContent();
        }
    }
}