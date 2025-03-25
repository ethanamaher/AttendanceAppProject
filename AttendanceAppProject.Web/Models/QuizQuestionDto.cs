namespace AttendanceAppProject.Web.Models;

public class QuizQuestionDto
{
	public Guid QuestionId { get; set; }

	public Guid QuizId { get; set; }

	public string CorrectAnswer { get; set; } = null!;

	public string QuestionText { get; set; } = null!;

	public string AnswerA { get; set; } = null!;

	public string AnswerB { get; set; } = null!;

	public string? AnswerC { get; set; }

	public string? AnswerD { get; set; }
}
