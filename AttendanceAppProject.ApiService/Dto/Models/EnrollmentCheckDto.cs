using AttendanceAppProject.ApiService.Models;
using System;

namespace AttendanceAppProject.ApiService.Dto.Models
{
    public class EnrollmentCheckDto
    {
        public StudentDto Student { get; set; }
        public ClassDto Class { get; set; }
    }
}