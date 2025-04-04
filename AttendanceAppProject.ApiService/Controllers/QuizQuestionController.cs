using Microsoft.AspNetCore.Mvc;
using AttendanceAppProject.ApiService.Data.Models;
using Microsoft.EntityFrameworkCore;
using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.Dto.Models;

// API Controller for Password

namespace AttendanceAppProject.ApiService.Controllers
{
	[Route("api/[controller]")] // Automatically becomes "api/password"
	[ApiController]
	public class QuizQuestionController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public QuizQuestionController(ApplicationDbContext context)
		{
			_context = context;
		}

		/* GET: api/quizquestions
         * Get all passwords
         * - request body: none
         * - response body: Passwords
         */
		[HttpGet("{QuizId}")]
		public async Task<ActionResult<IEnumerable<QuizQuestion>>> GetQuestionsById(Guid QuizId)
		{
			System.Diagnostics.Debug.WriteLine($"Looking up quiz {QuizId}");
			return await _context.QuizQuestions.Where(qi => qi.QuizId == QuizId).ToListAsync();
		}
	}
}

