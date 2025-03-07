using System;
using System.Collections.Generic;

namespace AttendanceAppProject.ApiService.Models;

public partial class ClassSchedule
{
    public Guid ClassScheduleId { get; set; }

    public Guid ClassId { get; set; }

    public string? DayOfWeek { get; set; }

    public virtual Class Class { get; set; } = null!;
}
