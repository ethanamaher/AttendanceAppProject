using System;

namespace AttendanceAppProject.ApiService.Dto.Models
{
    public class AttendanceInstanceDto
    {
        public string StudentId { get; set; }
        public Guid ClassId { get; set; }
        public string IpAddress { get; set; }
        public bool? IsLate { get; set; }
        public bool? ExcusedAbsence { get; set; }
        public DateTime? DateTime { get; set; }
    }
}