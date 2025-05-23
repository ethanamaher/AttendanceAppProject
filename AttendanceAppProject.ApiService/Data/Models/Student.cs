﻿namespace AttendanceAppProject.ApiService.Data.Models;

public partial class Student
{
    public string UtdId { get; set; } = null!; // PK

	public string FirstName { get; set; } = null!;

	public string LastName { get; set; } = null!;

	public string Username { get; set; } = null!;

	public virtual ICollection<AttendanceInstance> AttendanceInstances { get; set; } = new List<AttendanceInstance>();

    public virtual ICollection<QuizResponse> QuizResponses { get; set; } = new List<QuizResponse>();

    //added this for new StudentClass model
    public virtual ICollection<StudentClass> StudentClasses { get; set; } = new List<StudentClass>();
}
