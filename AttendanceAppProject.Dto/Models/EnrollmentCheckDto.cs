/* EnrollmentCheck Dto - This is a wrapper DTO, used by StudentClassController.cs to check if a student is enrolled in a class. 
 * It encapsulates a student DTO and a class DTO into a single DTO so the client side can send a single request to the server 
 * in order to check if a StudentClass exists containing the given StudentDto's id and the given ClassDto's id
 * Written by Maaz Raza
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AttendanceAppProject.Dto.Models
{
    public class EnrollmentCheckDto
    {
        public StudentDto Student { get; set; } = null!;
        public ClassDto Class { get; set; } = null!;
    }
}
