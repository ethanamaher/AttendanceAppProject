using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AttendanceAppProject.ApiService.Models;

namespace AttendanceAppProject.ApiService
{
    public interface IAttendanceService
    {
        Task<List<AttendanceRecord>> GetAttendanceRecordsAsync(string professorId);
        Task<List<AttendanceRecord>> GetAttendanceRecordsByClassAsync(string professorId, string className);
        Task<List<AttendanceRecord>> GetAttendanceRecordsByDateAsync(string professorId, DateTime date);
        Task<AttendanceRecord> GetAttendanceRecordByIdAsync(int id);
        Task<int> AddAttendanceRecordAsync(AttendanceRecord record);
    }
}