using System.Threading.Tasks;

namespace AttendanceAppProject.ApiService
{
    public interface IProfessorAuthService
    {
        Task<Professor> AuthenticateProfessorAsync(string professorId, string password);
        Task<bool> UpdateLastLoginAsync(string professorId);
    }
}