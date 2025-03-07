using System;
using System.Collections.Generic;

namespace AttendanceAppProject.Shared.Models;

public partial class QuizResponse
{
    public Guid ResponseId { get; set; }

    public string StudentId { get; set; } = null!;

    public Guid QuizQuestionId { get; set; }

    public Guid QuizInstanceId { get; set; }

    public virtual QuizInstance QuizInstance { get; set; } = null!;

    public virtual QuizQuestion QuizQuestion { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
