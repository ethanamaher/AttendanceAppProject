using System.Threading.Tasks;
using AttendanceAppProject.ApiService.Models;

namespace AttendanceAppProject.ApiService
{
    public interface IProfessorAuthService
    {
        Task<Professor> AuthenticateProfessorAsync(string professorId, string password);
        Task<Professor> GetProfessorByIdAsync(string professorId);
        bool VerifyPassword(string password, string passwordHash);
    }
}