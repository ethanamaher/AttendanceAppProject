using System;
using System.Collections.Generic;

namespace AttendanceAppProject.Dto.Models;

public class ClassScheduleDto
{
    public Guid ClassScheduleId { get; set; }

    public Guid ClassId { get; set; }

    public string? DayOfWeek { get; set; }
}
