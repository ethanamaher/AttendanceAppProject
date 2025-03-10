using System;
using System.Collections.Generic;

namespace AttendanceAppProject.ApiService.Data.Models;

public partial class QuizQuestion
{
    public Guid QuestionId { get; set; }

    public Guid QuizId { get; set; }

    public string CorrectAnswer { get; set; } = null!;

    public string QuestionText { get; set; } = null!;

    public string AnswerA { get; set; } = null!;

    public string AnswerB { get; set; } = null!;

    public string? AnswerC { get; set; }

    public string? AnswerD { get; set; }

    public virtual QuizInstance Quiz { get; set; } = null!;

    public virtual ICollection<QuizResponse> QuizResponses { get; set; } = new List<QuizResponse>();
}
