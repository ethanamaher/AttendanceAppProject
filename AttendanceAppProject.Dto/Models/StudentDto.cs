/* Student Dto, used to transfer student data between the server and client side and encapsulate JSON responses from HTTP requests for client to interact with 
 * Written by Maaz Raza
 */

using System;
using System.Collections.Generic;

namespace AttendanceAppProject.Dto.Models;

public class StudentDto
{
    public string UtdId { get; set; } = null!; // PK

	public string FirstName { get; set; } = null!;

	public string LastName { get; set; } = null!;

	public string Username { get; set; } = null!;
}
