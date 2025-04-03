using System;

namespace AttendanceAppProject.ApiService.Models
{
    public class AttendanceRecord
    {
        public int Id { get; set; }
        public string ProfessorId { get; set; } = string.Empty;
        public string ProfessorName { get; set; } = string.Empty;
        public string StudentId { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public DateTime CheckInTime { get; set; } = DateTime.Now;
        public string QuizQuestion { get; set; } = string.Empty;
        public string QuizAnswer { get; set; } = string.Empty;

        // This property is not stored in the database - it's only for UI display
        public int OrdinalNumber { get; set; }
    }
}