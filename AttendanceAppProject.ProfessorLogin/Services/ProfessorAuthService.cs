using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AttendanceAppProject.Dto.Models;
using AttendanceAppProject.ApiService.Data.Models;

namespace AttendanceAppProject.ProfessorLogin
{
    public interface IProfessorAuthService
    {
        Task<ProfessorDto> AuthenticateAsync(string professorId, string password);
    }

    public class ProfessorAuthService : IProfessorAuthService
    {
        private readonly HttpClient _httpClient;

        public ProfessorAuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ProfessorDto> AuthenticateAsync(string professorId, string password)
        {
            try
            {
                // Get professor from the API
                var response = await _httpClient.GetAsync($"api/Professor");

                if (response.IsSuccessStatusCode)
                {
                    var professors = await response.Content.ReadFromJsonAsync<Professor[]>();
                    var professor = Array.Find(professors, p => p.UtdId == professorId);

                    if (professor != null)
                    {
                        // For demo purposes, accept any password
                        // In a real application, the API would validate the password

                        return new ProfessorDto
                        {
                            UtdId = professor.UtdId,
                            FirstName = $"{professor.FirstName}",
                            LastName = $"{professor.LastName}",
                            Department = "Computer Science", // Hard-coded for demo
                            Email = $"{professor.FirstName.ToLower()}.{professor.LastName.ToLower()}@utdallas.edu"
                        };
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Authentication error: {ex.Message}");
                return null;
            }
        }

        private string CreatePasswordHash(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}