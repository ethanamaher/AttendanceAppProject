using System;
using System.Collections.Generic;

namespace AttendanceAppProject.Dto.Models;

public class ClassDto
{
    // Nullable so that the client doesn't need to provide an ID when creating a new attendance instance.
    // The server (API controller) will auto-generate a new GUID and assign it when saving to the database.
    // When retrieving data (from GET requests), this field will be populated with the actual value from the DB.
    public Guid? ClassId { get; set; } // PK

    public string ProfUtdId { get; set; } = null!; // FK

    public string? ClassPrefix { get; set; }

    public string? ClassNumber { get; set; }

    public string? ClassName { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

}
