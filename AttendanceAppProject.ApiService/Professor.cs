using System;
using System.ComponentModel.DataAnnotations;

namespace AttendanceAppProject.ApiService
{
    public class Professor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string ProfessorId { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string PasswordHash { get; set; } = string.Empty;

        [StringLength(50)]
        public string Department { get; set; } = string.Empty;

        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? LastLoginDate { get; set; }
    }
}