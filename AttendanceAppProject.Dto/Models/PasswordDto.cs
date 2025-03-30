using System;
using System.Collections.Generic;

namespace AttendanceAppProject.Dto.Models;

public class PasswordDto
{
    // Nullable so that the client doesn't need to provide an ID when creating a new attendance instance.
    // The server (API controller) will auto-generate a new GUID and assign it when saving to the database.
    // When retrieving data (from GET requests), this field will be populated with the actual value from the DB.
    public Guid? PasswordId { get; set; } // PK

    public Guid ClassId { get; set; } // FK

    public DateOnly? DateAssigned { get; set; }

    public string PasswordText { get; set; } = null!;
}
