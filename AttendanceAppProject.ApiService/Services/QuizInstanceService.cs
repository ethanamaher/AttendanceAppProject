using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.ApiService.Data.Models;
using AttendanceAppProject.Dto.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceAppProject.ApiService.Services
{
    public class QuizInstanceService
    {
        private readonly ApplicationDbContext _context;

        public QuizInstanceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<QuizInstance>> GetQuizzesAsync()
        {
            return await _context.QuizInstances.ToListAsync();
        }

        public async Task<QuizInstance> GetQuizByIdAsync(Guid ClassId)
        {
            return await _context.QuizInstances.FirstOrDefaultAsync(qi => qi.ClassId == ClassId);
        }

        public async Task<QuizInstance> AddQuizInstanceAsync(QuizInstanceDto dto)
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
            return newQuiz;
        }
    }
}
