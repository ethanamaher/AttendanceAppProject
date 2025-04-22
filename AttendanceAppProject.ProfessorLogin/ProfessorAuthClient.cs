using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AttendanceAppProject.ApiService.Data.Models;
using AttendanceAppProject.Dto.Models;

namespace AttendanceAppProject.ProfessorLogin
{
    public interface IProfessorAuthClient
    {
        Task<ProfessorDto?> LoginAsync(string utdId, string password);
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

        public async Task<ProfessorDto?> LoginAsync(string utdId, string password)
        {
            var dto = new ProfessorDto { UtdId = utdId, Password = password };
            var response = await _httpClient.PostAsJsonAsync("api/professor/login", dto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ProfessorDto>(_jsonOptions);
            }

            return null;
        }
    }
}