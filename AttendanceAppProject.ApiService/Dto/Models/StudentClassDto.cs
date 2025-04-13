using System;

namespace AttendanceAppProject.ApiService.Dto.Models
{
    public class StudentClassDto
    {
        public string StudentId { get; set; }
        public Guid ClassId { get; set; }
    }
}