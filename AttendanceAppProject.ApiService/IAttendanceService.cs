using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceAppProject.ApiService
{
    public interface IAttendanceService
    {
        Task<List<AttendanceRecord>> GetAttendanceRecordsAsync();
        Task<AttendanceRecord> GetAttendanceRecordByIdAsync(int id);
        Task<int> AddAttendanceRecordAsync(AttendanceRecord record);
        Task UpdateAttendanceRecordAsync(AttendanceRecord record);
        Task DeleteAttendanceRecordAsync(int id);
        Task<bool> SubmitAttendanceAsync(AttendanceRecord record);
    }
}