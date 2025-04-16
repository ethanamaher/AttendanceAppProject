using AttendanceAppProject.ApiService.Data;
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
		private readonly ApplicationDbContext _context;

		public QuizInstanceController(ApplicationDbContext context)
		{
			_context = context;
		}

		/* GET: api/QuizInstance
		 * Get all quizzes
		 * - request body: none
		 * - response body: QuizInstances
		 */
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Class>>> GetQuizzes()
		{
			return await _context.Classes.ToListAsync();
		}

		/* GET: api/QuizInstance/{id}
		 * Get quiz whose classId private key = id
		 * - request body: Guid classId
		 * - response body: QuizInstance
		 */
		[HttpGet("{ClassId}")]
		public async Task<ActionResult<QuizInstance>> GetQuizById(Guid ClassId)
		{
			var quizInstance = await _context.QuizInstances.FirstOrDefaultAsync(qi => qi.ClassId == ClassId);
			
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
			var newQuiz = new QuizInstance
			{
				QuizId = Guid.NewGuid(), // Auto-generate
				ClassId = dto.ClassId,
				StartTime = dto.StartTime,
				EndTime = dto.EndTime
			};

			_context.QuizInstances.Add(newQuiz);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetQuizzes), new { id = newQuiz.QuizId }, newQuiz);
		}
	}
}
