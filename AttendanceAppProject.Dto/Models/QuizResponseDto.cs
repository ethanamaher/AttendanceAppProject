/* Quiz Response Dto, used to transfer Quiz data between the server and client side and encapsulate JSON responses from HTTP requests for client to interact with 
 * Written by Maaz Raza
 */

using System;
using System.Collections.Generic;

namespace AttendanceAppProject.Dto.Models;

public partial class QuizResponseDto
{
    // Nullable so that the client doesn't need to provide an ID when creating a new QuizResponse.
    // The server (API controller) will auto-generate a new GUID and assign it when saving to the database.
    // When retrieving data (from GET requests), this field will be populated with the actual value from the DB.
    public Guid? ResponseId { get; set; } // PK

    public string StudentId { get; set; } = null!; // FK1

    public Guid QuizQuestionId { get; set; } // FK2

    public Guid QuizInstanceId { get; set; } // FK3
}
