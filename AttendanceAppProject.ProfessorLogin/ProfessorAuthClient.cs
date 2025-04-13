using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AttendanceAppProject.ApiService.Data.Models;
using AttendanceAppProject.ProfessorLogin.Models;

namespace AttendanceAppProject.ProfessorLogin
{
    public interface IProfessorAuthClient
    {
        Task<ProfessorModel> LoginAsync(string professorId, string password);
        Task<ProfessorModel> GetProfessorByIdAsync(string professorId);
    }

    public class ProfessorAuthClient : IProfessorAuthClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public ProfessorAuthClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ProfessorModel> LoginAsync(string professorId, string password)
        {
            try
            {
                // First, check if the professor exists
                var professorResponse = await _httpClient.GetAsync($"api/Professor");

                if (professorResponse.IsSuccessStatusCode)
                {
                    var professors = await professorResponse.Content.ReadFromJsonAsync<Professor[]>(_jsonOptions);
                    var professor = Array.Find(professors, p => p.UtdId == professorId);

                    if (professor != null)
                    {
                        // For demo purposes, we're accepting any password

                        return new ProfessorModel
                        {
                            ProfessorId = professor.UtdId,
                            FullName = $"{professor.FirstName} {professor.LastName}",
                            Department = "Computer Science", // Hard-coded for demo
                            Email = $"{professor.FirstName.ToLower()}.{professor.LastName.ToLower()}@utdallas.edu" // Generated
                        };
                    }
                }

                return null; // Authentication failed
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Authentication error: {ex.Message}");
                return null;
            }
        }

        public async Task<ProfessorModel> GetProfessorByIdAsync(string professorId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Professor");

                if (response.IsSuccessStatusCode)
                {
                    var professors = await response.Content.ReadFromJsonAsync<Professor[]>(_jsonOptions);
                    var professor = Array.Find(professors, p => p.UtdId == professorId);

                    if (professor != null)
                    {
                        return new ProfessorModel
                        {
                            ProfessorId = professor.UtdId,
                            FullName = $"{professor.FirstName} {professor.LastName}",
                            Department = "Computer Science", // Hard-coded for demo
                            Email = $"{professor.FirstName.ToLower()}.{professor.LastName.ToLower()}@utdallas.edu" // Generated
                        };
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting professor: {ex.Message}");
                return null;
            }
        }
    }
}