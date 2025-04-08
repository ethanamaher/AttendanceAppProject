namespace AttendanceAppProject.ApiService.Data.Models;

public partial class QuizAnswer
{

    public int AnswerId { get; set; } // PK
    public Guid QuestionId { get; set; } // FK

    public Guid QuizId { get; set; } // FK

    public string AnswerText { get; set; } = null!;
    
    public bool IsCorrect { get; set; }
}
