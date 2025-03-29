using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AttendanceAppProject.ApiService
{
    public class AttendanceApiClient : IAttendanceApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public AttendanceApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<AttendanceRecord>> GetAttendanceRecordsAsync()
        {
            var response = await _httpClient.GetAsync("api/attendance");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<AttendanceRecord>>(_jsonOptions) ?? new List<AttendanceRecord>();
        }

        public async Task<bool> SubmitAttendanceAsync(AttendanceRecord record)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(record),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("api/attendance", content);
            return response.IsSuccessStatusCode;
        }
    }
}