namespace AttendanceAppProject.Web.Models;

public class ClassScheduleDto
{
	public Guid ClassScheduleId { get; set; }

	public Guid ClassId { get; set; }

	public string? DayOfWeek { get; set; }
}
