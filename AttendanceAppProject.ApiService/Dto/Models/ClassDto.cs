using System;

namespace AttendanceAppProject.ApiService.Dto.Models
{
    public class ClassDto
    {
        public Guid ClassId { get; set; }
        public string ProfUtdId { get; set; }
        public string ClassPrefix { get; set; }
        public string ClassNumber { get; set; }
        public string ClassName { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
    }
}