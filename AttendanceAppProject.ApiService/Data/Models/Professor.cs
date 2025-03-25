namespace AttendanceAppProject.ApiService.Data.Models;

public partial class Professor
{
	public string UtdId { get; set; } = null!;

	public string FirstName { get; set; } = null!;

	public string LastName { get; set; } = null!;

	public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
}
