/* Professor Dto, used to transfer professor data between the server and client side and encapsulate JSON responses from HTTP requests for client to interact with 
 * Written by Maaz Raza
 */

using System;
using System.Collections.Generic;

namespace AttendanceAppProject.Dto.Models;

public class ProfessorDto
{
    public string UtdId { get; set; } = null!; // PK

	public string FirstName { get; set; } = string.Empty;

	public string LastName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
