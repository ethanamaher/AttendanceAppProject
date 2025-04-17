using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.ApiService.Data.Models;
using AttendanceAppProject.Dto.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceAppProject.ApiService.Services
{
    public class QuizAnswerService
    {
        private readonly ApplicationDbContext _context;

        public QuizAnswerService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get QuizAnswers by QuestionId
        public async Task<IEnumerable<QuizAnswer>> GetAnswersByIdAsync(Guid QuestionId)
        {
            return await _context.QuizAnswers.Where(qanswer => qanswer.QuestionId == QuestionId).ToListAsync();
        }
    }
}
