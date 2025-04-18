using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.ApiService.Services;
using AttendanceAppProject.ApiService.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AttendanceAppProject.Dto.Models;

// API controller for Quiz Instance

namespace AttendanceAppProject.ApiService.Controllers
{
    [Route("api/[controller]")] // Automatically becomes "api/QuizInstance"
    [ApiController]
    public class QuizInstanceController : ControllerBase
    {
        private readonly QuizInstanceService _service;

        public QuizInstanceController(QuizInstanceService service)
        {
            _service = service;
        }

        /* GET: api/QuizInstance
		 * Get all quizzes
		 * - request body: none
		 * - response body: QuizInstances
		 */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Class>>> GetQuizzes()
        {
            return Ok(await _service.GetQuizzesAsync());
        }

        /* GET: api/QuizInstance/{id}
		 * Get quiz whose classId private key = id
		 * - request body: Guid classId
		 * - response body: QuizInstance
		 */
        [HttpGet("{ClassId}")]
        public async Task<ActionResult<QuizInstance>> GetQuizById(Guid ClassId)
        {
            var quizInstance = await _service.GetQuizByIdAsync(ClassId);

            // check http response for quizinstance, if no quiz either just submit automatically or allow to submit
            if (quizInstance == null)
            {
                return NotFound();
            }

            return quizInstance;
        }



        /* POST: api/QuizInstance
         * Add a quiz instance to the database
         * - request body: QuizInstanceDto
         * - response body: QuizInstance
         */
        [HttpPost]
        public async Task<ActionResult<QuizInstance>> AddQuizInstance([FromBody] QuizInstanceDto dto)
        {
            var newQuiz = await _service.AddQuizInstanceAsync(dto);

            return CreatedAtAction(nameof(GetQuizzes), new { id = newQuiz.QuizId }, newQuiz);
        }
    }
}