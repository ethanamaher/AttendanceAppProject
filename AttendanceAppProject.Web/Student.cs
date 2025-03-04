namespace AttendanceAppProject.Web
{
  public class Student
  {
    public string id;
    public string name;

    public Student(string id, string name)
    {
      this.id = id;
      this.name = name;
    }

    public override string ToString()
    {
      return id + ", " + name;
    }
  }
}
