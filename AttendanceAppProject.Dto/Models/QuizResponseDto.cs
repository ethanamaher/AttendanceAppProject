using System;
using System.Collections.Generic;

namespace AttendanceAppProject.Dto.Models;

public partial class QuizResponseDto
{
    public Guid ResponseId { get; set; }

    public string StudentId { get; set; } = null!;

    public Guid QuizQuestionId { get; set; }

    public Guid QuizInstanceId { get; set; }
}
