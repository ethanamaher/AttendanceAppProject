using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AttendanceAppProject.ApiService
{
    public class AttendanceService : IAttendanceService
    {
        private readonly ApplicationDbContext _context;

        public AttendanceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AttendanceRecord>> GetAttendanceRecordsAsync()
        {
            return await _context.AttendanceRecords
                .OrderByDescending(r => r.CheckInTime)
                .ToListAsync();
        }

        public async Task<AttendanceRecord> GetAttendanceRecordByIdAsync(int id)
        {
            var record = await _context.AttendanceRecords.FindAsync(id);

            if (record == null)
            {
                throw new KeyNotFoundException($"Attendance record with ID {id} not found");
            }

            return record;
        }

        public async Task<int> AddAttendanceRecordAsync(AttendanceRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            // Set check-in time to current time if not provided
            if (record.CheckInTime == default)
            {
                record.CheckInTime = DateTime.Now;
            }

            await _context.AttendanceRecords.AddAsync(record);
            await _context.SaveChangesAsync();

            return record.Id;
        }

        public async Task UpdateAttendanceRecordAsync(AttendanceRecord record)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            var existingRecord = await _context.AttendanceRecords.FindAsync(record.Id);

            if (existingRecord == null)
            {
                throw new KeyNotFoundException($"Attendance record with ID {record.Id} not found");
            }

            // Update properties
            existingRecord.ProfessorName = record.ProfessorName;
            existingRecord.StudentId = record.StudentId;
            existingRecord.Class = record.Class;
            existingRecord.QuizQuestion = record.QuizQuestion;
            existingRecord.QuizAnswer = record.QuizAnswer;

            _context.AttendanceRecords.Update(existingRecord);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAttendanceRecordAsync(int id)
        {
            var record = await _context.AttendanceRecords.FindAsync(id);

            if (record == null)
            {
                throw new KeyNotFoundException($"Attendance record with ID {id} not found");
            }

            _context.AttendanceRecords.Remove(record);
            await _context.SaveChangesAsync();
        }

        // Implement the SubmitAttendanceAsync method to match what the API client expects
        public async Task<bool> SubmitAttendanceAsync(AttendanceRecord record)
        {
            try
            {
                // If the record has an ID, update it; otherwise, add it
                if (record.Id > 0)
                {
                    await UpdateAttendanceRecordAsync(record);
                }
                else
                {
                    await AddAttendanceRecordAsync(record);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}