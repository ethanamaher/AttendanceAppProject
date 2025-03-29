using System;
using System.Collections.Generic;

namespace AttendanceAppProject.Dto.Models;

public partial class QuizInstanceDto
{
    public Guid QuizId { get; set; } // PK

    public Guid ClassId { get; set; } // FK

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }
}
