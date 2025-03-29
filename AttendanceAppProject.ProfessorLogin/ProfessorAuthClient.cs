using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AttendanceAppProject.ProfessorLogin
{
    public interface IProfessorAuthClient
    {
        Task<ProfessorDto> LoginAsync(string professorId, string password);
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

        public async Task<ProfessorDto> LoginAsync(string professorId, string password)
        {
            var loginRequest = new
            {
                ProfessorId = professorId,
                Password = password
            };

            var content = new StringContent(
                JsonSerializer.Serialize(loginRequest),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("api/ProfessorAuth/login", content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ProfessorDto>(_jsonOptions);
            }

            return null;
        }
    }

    public class ProfessorDto
    {
        public string ProfessorId { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public string LastLogin { get; set; }
    }
}