using System;

namespace AttendanceAppProject.ApiService.Dto.Models
{
    public class PasswordDto
    {
        public Guid ClassId { get; set; }
        public DateOnly? DateAssigned { get; set; }
        public string PasswordText { get; set; }
    }
}