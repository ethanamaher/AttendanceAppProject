using System;
using System.Collections.Generic;

namespace AttendanceAppProject.Dto.Models;

public class PasswordDto
{
    public Guid PasswordId { get; set; } // PK

    public Guid ClassId { get; set; } // FK

    public DateOnly? DateAssigned { get; set; }

    public string PasswordText { get; set; } = null!;
}
