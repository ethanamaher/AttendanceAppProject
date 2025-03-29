using System;
using System.Collections.Generic;

namespace AttendanceAppProject.ApiService.Data.Models;

public partial class QuizInstance
{
    public Guid QuizId { get; set; } // PK

    public Guid ClassId { get; set; } // FK

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();

    public virtual ICollection<QuizResponse> QuizResponses { get; set; } = new List<QuizResponse>();
}
