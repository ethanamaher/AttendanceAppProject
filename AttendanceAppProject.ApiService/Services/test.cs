using AttendanceAppProject.ApiService.Models;

namespace AttendanceAppProject.ApiService
{
   public interface IAttendanceApiClient
    {
        Task<List<AttendanceRecord>> GetAttendanceRecordsAsync();
        Task<bool> SubmitAttendanceAsync(AttendanceRecord record);
    }
}
