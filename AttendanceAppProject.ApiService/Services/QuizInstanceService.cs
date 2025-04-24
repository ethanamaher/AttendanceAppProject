using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.ApiService.Data.Models;
using AttendanceAppProject.Dto.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceAppProject.ApiService.Services
{
    public class QuizInstanceService
    {
        private readonly ApplicationDbContext _context;

        // Constructor to initialize the ApplicationDbContext
        public QuizInstanceService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all quiz instances (quizzes) in the system
        public async Task<IEnumerable<QuizInstance>> GetQuizzesAsync()
        {
            return await _context.QuizInstances.ToListAsync();
        }

        // Get a quiz instance by ClassId
        public async Task<QuizInstance> GetQuizByIdAsync(Guid ClassId)
        {
            return await _context.QuizInstances.FirstOrDefaultAsync(qi => qi.ClassId == ClassId);
        }

        // Add a new quiz instance (quiz) to the database
        public async Task<QuizInstance> AddQuizInstanceAsync(QuizInstanceDto dto)
        {
            var newQuiz = new QuizInstance
            {
                QuizId = Guid.NewGuid(), // Auto-generate the QuizId
                ClassId = dto.ClassId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime
            };

            _context.QuizInstances.Add(newQuiz);
            await _context.SaveChangesAsync();
            return newQuiz;
        }

        // Update the class for an existing quiz instance
        public async Task<QuizInstance> UpdateQuizClassAsync(Guid QuizId, Guid newClassId)
        {
            var quiz = await _context.QuizInstances.FindAsync(QuizId);

            if (quiz == null)
            {
                return null;
            }

            quiz.ClassId = newClassId;
            await _context.SaveChangesAsync();

            return quiz;
        }

        // Delete a quiz instance from the database
        public async Task<bool> DeleteQuizInstanceAsync(Guid QuizId)
        {
            var quiz = await _context.QuizInstances.FindAsync(QuizId);

            if (quiz == null)
            {
                return false;
            }

            _context.QuizInstances.Remove(quiz);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}