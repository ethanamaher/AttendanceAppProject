// Canh Nguyen

namespace AttendanceAppProject.ProfessorLogin.Models
{
    public class ProfessorModel
    {
        // Match properties from the database model
        public string ProfessorId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        // We don't need to include PasswordHash or Id for the UI
    }
}