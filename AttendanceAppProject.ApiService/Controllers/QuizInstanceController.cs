using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.ApiService.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AttendanceAppProject.Dto.Models;

// API controller for Class

namespace AttendanceAppProject.ApiService.Controllers
{
	[Route("api/[controller]")] // Automatically becomes "api/class"
	[ApiController]
	public class QuizInstanceController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public QuizInstanceController(ApplicationDbContext context)
		{
			_context = context;
		}

		/* GET: api/Class
		 * Get all classes
		 * - request body: none
		 * - response body: Classes
		 */
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Class>>> GetQuizzes()
		{
			return await _context.Classes.ToListAsync();
		}

		/* GET: api/Class/{id}
		 * Get class whose classId private key = id
		 * - request body: Guid classId
		 * - response body: Class
		 */
		[HttpGet("{ClassId}")]
		public async Task<ActionResult<QuizInstance>> GetQuizById(Guid ClassId)
		{
			System.Diagnostics.Debug.WriteLine($"-----Quiz CONTROLLER-----");
			System.Diagnostics.Debug.WriteLine($"Getting Quiz For {ClassId}");
			var quizInstance = await _context.QuizInstances.FirstOrDefaultAsync(qi => qi.ClassId == ClassId);
			System.Diagnostics.Debug.WriteLine($"{quizInstance.QuizId.ToString()}");

			if (quizInstance == null)
			{
				return NotFound();
			}

			return quizInstance;
		}



		/* POST: api/Class
         * Add a class to the database
         * - request body: ClassDto
         * - response body: Class
         */
		[HttpPost]
		public async Task<ActionResult<Class>> AddClass([FromBody] ClassDto dto)
		{
			var newClass = new Class
			{
				ClassId = Guid.NewGuid(), // Auto-generate
				ProfUtdId = dto.ProfUtdId,
				ClassPrefix = dto.ClassPrefix,
				ClassNumber = dto.ClassNumber,
				ClassName = dto.ClassName,
				StartDate = dto.StartDate,
				EndDate = dto.EndDate,
				StartTime = dto.StartTime,
				EndTime = dto.EndTime
			};

			_context.Classes.Add(newClass);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetQuizzes), new { id = newClass.ClassId }, newClass);
		}
	}
}
