using System.Threading.Tasks;
using AttendanceAppProject.ApiService.Models; // Using models from ApiService.Models namespace

namespace AttendanceAppProject.ApiService
{
    public interface IProfessorAuthService
    {
        Task<Models.Professor> AuthenticateProfessorAsync(string professorId, string password);
        Task<Models.Professor> GetProfessorByIdAsync(string professorId);
        bool VerifyPassword(string password, string passwordHash);
    }
}