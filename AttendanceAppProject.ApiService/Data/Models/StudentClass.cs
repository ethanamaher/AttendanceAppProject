namespace AttendanceAppProject.ApiService.Data.Models
{
    public partial class StudentClass
    {
        public Guid StudentClassId { get; set; }  // PK

        public string StudentId { get; set; } = null!;  // FK to Student

        public Guid ClassId { get; set; }           // FK to Class

        public virtual Student Student { get; set; } = null!;
        public virtual Class Class { get; set; } = null!;
    }
}