using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.ApiService.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceAppProject.ApiService.Services
{
    public class QuizQuestionService
    {
        private readonly ApplicationDbContext _context;

        public QuizQuestionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<QuizQuestion>> GetQuestionsByIdAsync(Guid QuizId)
        {
            return await _context.QuizQuestions
                .Where(qi => qi.QuizId == QuizId)
                .ToListAsync();
        }
    }
}
