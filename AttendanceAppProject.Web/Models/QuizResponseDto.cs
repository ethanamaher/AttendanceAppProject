namespace AttendanceAppProject.Web.Models;

public partial class QuizResponseDto
{
	public Guid ResponseId { get; set; }

	public string StudentId { get; set; } = null!;

	public Guid QuizQuestionId { get; set; }

	public Guid QuizInstanceId { get; set; }
}
