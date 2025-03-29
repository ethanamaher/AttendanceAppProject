using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceAppProject.ApiService
{
    public interface IAttendanceApiClient
    {
        Task<List<AttendanceRecord>> GetAttendanceRecordsAsync();
        Task<bool> SubmitAttendanceAsync(AttendanceRecord record);
    }
}