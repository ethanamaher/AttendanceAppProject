using System;
using System.Collections.Generic;

namespace AttendanceAppProject.Dto.Models;

public class QuizQuestionDto
{
    public Guid QuestionId { get; set; } // PK

    public Guid QuizId { get; set; } // FK

    public string CorrectAnswer { get; set; } = null!;

    public string QuestionText { get; set; } = null!;

    public string AnswerA { get; set; } = null!;

    public string AnswerB { get; set; } = null!;

    public string? AnswerC { get; set; }

    public string? AnswerD { get; set; }
}
