namespace AttendanceAppProject.ApiService.Models
{
    public class Professor
    {
        public int Id { get; set; }
        public string ProfessorId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}