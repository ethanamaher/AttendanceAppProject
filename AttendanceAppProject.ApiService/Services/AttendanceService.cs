using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.ApiService.Models;

namespace AttendanceAppProject.ApiService
{
    public class AttendanceService : IAttendanceService
    {
        private readonly ApplicationDbContext _context;

        public AttendanceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AttendanceRecord>> GetAttendanceRecordsAsync(string professorId)
        {
            return await _context.AttendanceRecords
                .Where(r => r.ProfessorId == professorId)
                .OrderByDescending(r => r.CheckInTime)
                .ToListAsync();
        }

        public async Task<List<AttendanceRecord>> GetAttendanceRecordsByClassAsync(string professorId, string className)
        {
            return await _context.AttendanceRecords
                .Where(r => r.ProfessorId == professorId && r.Class == className)
                .OrderByDescending(r => r.CheckInTime)
                .ToListAsync();
        }

        public async Task<List<AttendanceRecord>> GetAttendanceRecordsByDateAsync(string professorId, DateTime date)
        {
            return await _context.AttendanceRecords
                .Where(r => r.ProfessorId == professorId &&
                      r.CheckInTime.Date == date.Date)
                .OrderByDescending(r => r.CheckInTime)
                .ToListAsync();
        }

        public async Task<AttendanceRecord> GetAttendanceRecordByIdAsync(int id)
        {
            return await _context.AttendanceRecords.FindAsync(id);
        }

        public async Task<int> AddAttendanceRecordAsync(AttendanceRecord record)
        {
            _context.AttendanceRecords.Add(record);
            await _context.SaveChangesAsync();
            return record.Id;
        }
    }
}