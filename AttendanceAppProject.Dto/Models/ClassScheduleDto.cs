using System;
using System.Collections.Generic;

namespace AttendanceAppProject.Dto.Models;

public class ClassScheduleDto
{
    public Guid ClassScheduleId { get; set; } // PK 

    public Guid ClassId { get; set; } // FK

    public string? DayOfWeek { get; set; }
}
