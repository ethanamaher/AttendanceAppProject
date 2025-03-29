using System;
using System.Collections.Generic;

namespace AttendanceAppProject.Dto.Models;

public partial class QuizResponseDto
{
    public Guid ResponseId { get; set; } // PK

    public string StudentId { get; set; } = null!; // FK1

    public Guid QuizQuestionId { get; set; } // FK2

    public Guid QuizInstanceId { get; set; } // FK3
}
