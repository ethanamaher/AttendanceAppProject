using System;
using System.Collections.Generic;

namespace AttendanceAppProject.Dto.Models;

public class StudentDto
{
    public string UtdId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Username { get; set; } = null!;
}
