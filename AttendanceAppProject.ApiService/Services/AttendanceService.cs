using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.ApiService.Models;
using AttendanceAppProject.ApiService.Data.Models;

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
            // Since AttendanceRecords doesn't exist in the DbContext, we need to use AttendanceInstances
            // and transform them to AttendanceRecord objects
            var instances = await _context.AttendanceInstances
                .Include(a => a.Class)
                .Include(a => a.Student)
                .Where(a => a.Class.ProfUtdId == professorId)
                .OrderByDescending(a => a.DateTime)
                .ToListAsync();

            return instances.Select(ConvertToAttendanceRecord).ToList();
        }

        public async Task<List<AttendanceRecord>> GetAttendanceRecordsByClassAsync(string professorId, string className)
        {
            var instances = await _context.AttendanceInstances
                .Include(a => a.Class)
                .Include(a => a.Student)
                .Where(a => a.Class.ProfUtdId == professorId &&
                           (a.Class.ClassName == className ||
                            a.Class.ClassPrefix + " " + a.Class.ClassNumber == className))
                .OrderByDescending(a => a.DateTime)
                .ToListAsync();

            return instances.Select(ConvertToAttendanceRecord).ToList();
        }

        public async Task<List<AttendanceRecord>> GetAttendanceRecordsByDateAsync(string professorId, DateTime date)
        {
            var instances = await _context.AttendanceInstances
                .Include(a => a.Class)
                .Include(a => a.Student)
                .Where(a => a.Class.ProfUtdId == professorId &&
                           a.DateTime.HasValue &&
                           a.DateTime.Value.Date == date.Date)
                .OrderByDescending(a => a.DateTime)
                .ToListAsync();

            return instances.Select(ConvertToAttendanceRecord).ToList();
        }

        public async Task<AttendanceRecord> GetAttendanceRecordByIdAsync(int id)
        {
            // Since our AttendanceInstance uses Guid, we'll need to adapt this method
            // For now, we'll return null
            return null;
        }

        public async Task<int> AddAttendanceRecordAsync(AttendanceRecord record)
        {
            // Create a new AttendanceInstance from the record
            var instance = new AttendanceInstance
            {
                AttendanceId = Guid.NewGuid(),
                StudentId = record.StudentId,
                ClassId = Guid.Parse(record.Class), // This assumes Class contains a valid Guid string
                IpAddress = "127.0.0.1",
                IsLate = record.IsLate,
                ExcusedAbsence = false,
                DateTime = record.CheckInTime
            };

            _context.AttendanceInstances.Add(instance);
            await _context.SaveChangesAsync();

            // Return a dummy ID since we don't have int IDs
            return 1;
        }

        private AttendanceRecord ConvertToAttendanceRecord(AttendanceInstance instance)
        {
            return new AttendanceRecord
            {
                // Since AttendanceInstance has a Guid ID, we'll just use a placeholder
                Id = 0,
                ProfessorId = instance.Class.ProfUtdId,
                ProfessorName = instance.Class.ProfUtd?.FirstName + " " + instance.Class.ProfUtd?.LastName,
                StudentId = instance.StudentId,
                Class = instance.Class.ClassPrefix + " " + instance.Class.ClassNumber,
                CheckInTime = instance.DateTime ?? DateTime.Now,
                QuizQuestion = "Default Question", // This would come from QuizInstance/QuizQuestion if needed
                QuizAnswer = "Default Answer",     // This would come from QuizResponse if needed
                // Additional fields can be added here
            };
        }
    }
}