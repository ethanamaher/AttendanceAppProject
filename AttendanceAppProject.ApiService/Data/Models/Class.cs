using System;
using System.Collections.Generic;

namespace AttendanceAppProject.ApiService.Data.Models;

public partial class Class
{
    public Guid ClassId { get; set; }

    public string ProfUtdId { get; set; } = null!;

    public string? ClassPrefix { get; set; }

    public string? ClassNumber { get; set; }

    public string? ClassName { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public virtual ICollection<AttendanceInstance> AttendanceInstances { get; set; } = new List<AttendanceInstance>();

    public virtual ICollection<ClassSchedule> ClassSchedules { get; set; } = new List<ClassSchedule>();

    public virtual ICollection<Password> Passwords { get; set; } = new List<Password>();

    public virtual Professor ProfUtd { get; set; } = null!;

    public virtual ICollection<QuizInstance> QuizInstances { get; set; } = new List<QuizInstance>();
}
