using System;
using System.Collections.Generic;

namespace AttendanceAppProject.Dto.Models;

public partial class QuizInstanceDto
{
    public Guid QuizId { get; set; }

    public Guid ClassId { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }
}
