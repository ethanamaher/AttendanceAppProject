// Canh Nguyen

using System;

namespace AttendanceAppProject.ProfessorLogin.Models
{
    public class AttendanceRecord
    {
        // Include Id property from the database model
        public int Id { get; set; }
        public int OrdinalNumber { get; set; }

        // Match properties from the database model
        public string ProfessorId { get; set; } = string.Empty;
        public string ProfessorName { get; set; } = string.Empty;
        public string StudentId { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public DateTime CheckInTime { get; set; } = DateTime.Now;
        public string QuizQuestion { get; set; } = string.Empty;
        public string QuizAnswer { get; set; } = string.Empty;

        // Additional properties for enhanced UI
        public bool IsLate { get; set; }
    }
}