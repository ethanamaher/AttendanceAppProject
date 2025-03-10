using System;
using System.Collections.Generic;

namespace AttendanceAppProject.Shared.Models;

public class PasswordDto
{
    public Guid PasswordId { get; set; }

    public Guid ClassId { get; set; }

    public DateOnly? DateAssigned { get; set; }

    public string PasswordText { get; set; } = null!;
}
