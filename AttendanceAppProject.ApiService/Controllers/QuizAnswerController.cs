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
    public class QuizAnswerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public QuizAnswerController(ApplicationDbContext context)
        {
            _context = context;
        }

        /* GET: api/quizquestions
         * Get all passwords
         * - request body: none
         * - response body: Passwords
         */
        [HttpGet("{QuestionId}")]
        public async Task<ActionResult<IEnumerable<QuizQuestion>>> GetAnswersById(Guid QuestionId)
        {
            System.Diagnostics.Debug.WriteLine($"Looking up answers for {QuestionId}");
            return await _context.QuizQuestions.Where(qanswer => qanswer.QuestionId == QuestionId).ToListAsync();
        }
    }
}

