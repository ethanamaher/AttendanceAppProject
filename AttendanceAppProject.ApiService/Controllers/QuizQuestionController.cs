using Microsoft.AspNetCore.Mvc;
using AttendanceAppProject.ApiService.Data.Models;
using Microsoft.EntityFrameworkCore;
using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.Dto.Models;
using AttendanceAppProject.ApiService.Services;

// API Controller for Password

namespace AttendanceAppProject.ApiService.Controllers
{
	[Route("api/[controller]")] // Automatically becomes "api/password"
	[ApiController]
	public class QuizQuestionController : ControllerBase
	{
		private readonly QuizQuestionService _service;

		public QuizQuestionController(QuizQuestionService service)
		{
			_service = service;
		}

        /* GET: api/quizquestions
         * Get quiz questions by id
         * - request body: none
         * - response body: <IEnumerable<QuizQuestion>
         */
        [HttpGet("{QuizId}")]
		public async Task<ActionResult<IEnumerable<QuizQuestion>>> GetQuestionsById(Guid QuizId)
		{
			System.Diagnostics.Debug.WriteLine($"Looking up quiz {QuizId}");
			return Ok(_service.GetQuestionsByIdAsync(QuizId));
		}
	}
}

