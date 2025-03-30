using System;
using System.Collections.Generic;

namespace AttendanceAppProject.Dto.Models;

/* DTO (data transfer object) for AttendanceInstance
 * DTOs are used to transfer data between the server and client side without the server side having to share the database models directly
 * These only contain relevant data for the client side, and are used to encapsulate the data into a single object that can be easily interacted with
 */ 

public class AttendanceInstanceDto
{
    // Nullable so that the client doesn't need to provide an ID when creating a new attendance instance.
    // The server (API controller) will auto-generate a new GUID and assign it when saving to the database.
    // When retrieving data (from GET requests), this field will be populated with the actual value from the DB.
    public Guid? AttendanceId { get; set; } // PK

    public string StudentId { get; set; } = null!; // FK1

    public Guid ClassId { get; set; } // FK2

    public string? IpAddress { get; set; }

    public bool? IsLate { get; set; }

    public bool? ExcusedAbsence { get; set; }

    public DateTime? DateTime { get; set; }
}
