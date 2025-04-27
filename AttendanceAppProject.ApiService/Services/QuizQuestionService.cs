using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.ApiService.Data.Models;
using AttendanceAppProject.Dto.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceAppProject.ApiService.Services
{
    public class QuizQuestionService
    {
        private readonly ApplicationDbContext _context;

        // Constructor to initialize the ApplicationDbContext
        public QuizQuestionService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all quiz questions for a given quiz ID
        public async Task<IEnumerable<QuizQuestion>> GetQuestionsByIdAsync(Guid QuizId)
        {
            return await _context.QuizQuestions
                .Where(qi => qi.QuizId == QuizId)
                .ToListAsync();
        }

        // Add a new quiz question
        public async Task<QuizQuestion> AddQuizQuestionAsync(QuizQuestionDto dto)
        {
            var newQuestion = new QuizQuestion
            {
                QuestionId = Guid.NewGuid(), // Auto-generate QuestionId
                QuizId = dto.QuizId,
                QuestionText = dto.QuestionText
            };

            _context.QuizQuestions.Add(newQuestion);
            await _context.SaveChangesAsync();

            return newQuestion;
        }

        // Delete a quiz question
        public async Task<bool> DeleteQuizQuestionAsync(Guid QuestionId)
        {
            var question = await _context.QuizQuestions.FindAsync(QuestionId);

            if (question == null)
            {
                return false;
            }

            _context.QuizQuestions.Remove(question);
            await _context.SaveChangesAsync();
            return true;
        }

        // Update a quiz question by QuestionId
        public async Task<QuizQuestion?> UpdateQuizQuestionAsync(Guid questionId, QuizQuestionDto updatedQuestion)
        {
            var quizQuestion = await _context.QuizQuestions.FindAsync(questionId);
            if (quizQuestion == null)
            {
                return null;
            }

            quizQuestion.QuestionText = updatedQuestion.QuestionText ?? quizQuestion.QuestionText;
            quizQuestion.QuizId = updatedQuestion.QuizId != Guid.Empty ? updatedQuestion.QuizId : quizQuestion.QuizId;

            await _context.SaveChangesAsync();
            return quizQuestion;
        }
    }
}