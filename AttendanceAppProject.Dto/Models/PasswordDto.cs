/* Password Dto, used to transfer password data between the server and client side and encapsulate JSON responses from HTTP requests for client to interact with 
 * Written by Maaz Raza
 */

using System;
using System.Collections.Generic;

namespace AttendanceAppProject.Dto.Models;

public class PasswordDto
{
    // Nullable so that the client doesn't need to provide an ID when creating a new password.
    // The server (API controller) will auto-generate a new GUID and assign it when saving to the database.
    // When retrieving data (from GET requests), this field will be populated with the actual value from the DB.
    public Guid? PasswordId { get; set; } // PK

    public Guid ClassId { get; set; } // FK

    public DateOnly? DateAssigned { get; set; }

    public string PasswordText { get; set; } = null!;
}
