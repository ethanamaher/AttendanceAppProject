using System;

namespace AttendanceAppProject.ApiService
{
    public class AttendanceRecord
    {
        public int Id { get; set; }
        public int OrdinalNumber { get; set; } // For UI display only, not stored in DB
        public string ProfessorName { get; set; } = string.Empty;
        public string StudentId { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public DateTime CheckInTime { get; set; }
        public string QuizQuestion { get; set; } = string.Empty;
        public string QuizAnswer { get; set; } = string.Empty;
    }
}