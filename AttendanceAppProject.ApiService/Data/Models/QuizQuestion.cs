namespace AttendanceAppProject.ApiService.Data.Models;

public partial class QuizQuestion
{
    public Guid QuestionId { get; set; } // PK

    public Guid QuizId { get; set; } // FK

	public string QuestionText { get; set; } = null!;

	public virtual QuizInstance Quiz { get; set; } = null!;

	public virtual ICollection<QuizResponse> QuizResponses { get; set; } = new List<QuizResponse>();
}
