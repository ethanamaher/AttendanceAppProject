using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.ApiService.Data.Models;
using AttendanceAppProject.Dto.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceAppProject.ApiService.Services
{
    public class QuizAnswerService
    {
        private readonly ApplicationDbContext _context;

        // Constructor to initialize the ApplicationDbContext
        public QuizAnswerService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get quiz answers by QuestionId
        public async Task<IEnumerable<QuizAnswer>> GetAnswersByIdAsync(Guid QuestionId)
        {
            return await _context.QuizAnswers.Where(qanswer => qanswer.QuestionId == QuestionId).ToListAsync();
        }

        // Add a new quiz answer
        public async Task<QuizAnswer> AddQuizAnswerAsync(QuizAnswerDto dto)
        {
            var newAnswer = new QuizAnswer
            {
                // Auto-generate AnswerId in the database side
                QuestionId = (Guid)dto.QuestionId,
                QuizId = (Guid)dto.QuizId,
                AnswerText = dto.AnswerText,
                IsCorrect = (bool)dto.IsCorrect
            };

            _context.QuizAnswers.Add(newAnswer);
            await _context.SaveChangesAsync();

            return newAnswer;
        }

        // Delete a quiz answer
        public async Task<bool> DeleteQuizAnswerAsync(int AnswerId)
        {
            var answer = await _context.QuizAnswers.FindAsync(AnswerId);

            if (answer == null)
            {
                return false;
            }

            _context.QuizAnswers.Remove(answer);
            await _context.SaveChangesAsync();
            return true;
        }

        // Update a quiz answer by AnswerId
        public async Task<QuizAnswer?> UpdateQuizAnswerAsync(int answerId, QuizAnswerDto updatedAnswer)
        {
            var quizAnswer = await _context.QuizAnswers.FindAsync(answerId);
            if (quizAnswer == null)
            {
                return null;
            }

            quizAnswer.AnswerText = updatedAnswer.AnswerText ?? quizAnswer.AnswerText;
            quizAnswer.IsCorrect = updatedAnswer.IsCorrect ?? quizAnswer.IsCorrect;

            await _context.SaveChangesAsync();
            return quizAnswer;
        }
    }
}