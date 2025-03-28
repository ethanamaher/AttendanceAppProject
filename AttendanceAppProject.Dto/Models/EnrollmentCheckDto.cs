using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// this is a wrapper DTO, used by StudentClassController.cs to check if a student is enrolled in a class
// it encapsulates an input of a student DTO and a class DTO, which can then be deserialized by the API controller
// in order to check if a StudentClass exists containing the given student's id and the given class's id

namespace AttendanceAppProject.Dto.Models
{
    public class EnrollmentCheckDto
    {
        public StudentDto Student { get; set; } = null!;
        public ClassDto Class { get; set; } = null!;
    }
}
