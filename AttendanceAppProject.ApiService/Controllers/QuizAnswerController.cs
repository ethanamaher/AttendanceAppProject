using Microsoft.AspNetCore.Mvc;
using AttendanceAppProject.ApiService.Data.Models;
using Microsoft.EntityFrameworkCore;
using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.Dto.Models;

// API Controller for Quiz Answers

namespace AttendanceAppProject.ApiService.Controllers
{
    [Route("api/[controller]")] // Automatically becomes "api/QuizAnswers"
    [ApiController]
    public class QuizAnswerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public QuizAnswerController(ApplicationDbContext context)
        {
            _context = context;
        }

        /* GET: api/QuizAnswers
         * Get quiz answers by the question Id
         * - request body: QuestionId
         * - response body: QuizAnswers
         */
        [HttpGet("{QuestionId}")]
        public async Task<ActionResult<IEnumerable<QuizAnswer>>> GetAnswersById(Guid QuestionId)
        {
            System.Diagnostics.Debug.WriteLine($"Looking up answers for {QuestionId}");
            return await _context.QuizAnswers.Where(qanswer => qanswer.QuestionId == QuestionId).ToListAsync();
        }
    }
}

