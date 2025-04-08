/* Quiz Question Dto, used to transfer QuizQuestion data between the server and client side and encapsulate JSON responses from HTTP requests for client to interact with 
 * Written by Maaz Raza
 */

using System;
using System.Collections.Generic;

namespace AttendanceAppProject.Dto.Models;

public class QuizAnswerDto
{
    // Nullable so that the client doesn't need to provide an ID when creating a new QuizQuestion.
    // The server (API controller) will auto-generate a new GUID and assign it when saving to the database.
    // When retrieving data (from GET requests), this field will be populated with the actual value from the DB.
    public int? AnswerId { get; set; }
    public Guid? QuizId { get; set; }
    public Guid? QuestionId { get; set; }

    public string? AnswerText { get; set; }
    public bool? IsCorrect { get; set; }


}
