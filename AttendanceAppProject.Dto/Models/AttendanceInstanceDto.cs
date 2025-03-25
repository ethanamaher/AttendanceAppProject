using System;
using System.Collections.Generic;

namespace AttendanceAppProject.Dto.Models;

public class AttendanceInstanceDto
{
    public Guid AttendanceId { get; set; }

    public string StudentId { get; set; } = null!;

    public Guid ClassId { get; set; }

    public string? IpAddress { get; set; }

    public bool? IsLate { get; set; }

    public bool? ExcusedAbsence { get; set; }

    public DateTime? DateTime { get; set; }
}
