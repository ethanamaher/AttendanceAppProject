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

    public Boolean Exists()
    {
      return id?.Length > 0 && name?.Length > 0;
    }

    public override string ToString()
    {
      return id + ", " + name;
    }
  }
}
