using System;
using System.Collections.Generic;

namespace AttendanceAppProject.Dto.Models;

public class QuizQuestionDto
{
    // Nullable so that the client doesn't need to provide an ID when creating a new attendance instance.
    // The server (API controller) will auto-generate a new GUID and assign it when saving to the database.
    // When retrieving data (from GET requests), this field will be populated with the actual value from the DB.
    public Guid? QuestionId { get; set; } // PK

    public Guid QuizId { get; set; } // FK

    public string CorrectAnswer { get; set; } = null!;

    public string QuestionText { get; set; } = null!;

    public string AnswerA { get; set; } = null!;

    public string AnswerB { get; set; } = null!;

    public string? AnswerC { get; set; }

    public string? AnswerD { get; set; }
}
