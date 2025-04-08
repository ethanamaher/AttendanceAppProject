namespace AttendanceAppProject.ApiService.Data.Models;

public partial class Professor
{
    public string UtdId { get; set; } = null!; // PK

	public string FirstName { get; set; } = null!;

	public string LastName { get; set; } = null!;

    public string PasswordHash { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
}
