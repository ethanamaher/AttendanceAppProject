using System;
using System.Collections.Generic;

namespace AttendanceAppProject.ApiService.Data.Models;

public partial class QuizResponse
{
    public Guid ResponseId { get; set; } // PK

    public string StudentId { get; set; } = null!; // FK1

    public Guid QuizQuestionId { get; set; } // FK2

    public Guid QuizInstanceId { get; set; } // FK3

    public virtual QuizInstance QuizInstance { get; set; } = null!;

    public virtual QuizQuestion QuizQuestion { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
