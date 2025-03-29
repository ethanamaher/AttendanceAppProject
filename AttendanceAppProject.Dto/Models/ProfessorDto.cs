using System;
using System.Collections.Generic;

namespace AttendanceAppProject.Dto.Models;

public class ProfessorDto
{
    public string UtdId { get; set; } = null!; // PK

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;
}
