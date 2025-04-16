/* API Service for Attendance Instance - written to handle class-related operations and delegate logic separate to API controller for better organization
 * Written by Maaz Raza
 */

using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.ApiService.Data.Models;
using AttendanceAppProject.Dto.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceAppProject.ApiService.Services
{
    public class AttendanceInstanceService
    {
        private readonly ApplicationDbContext _context;

        public AttendanceInstanceService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all attendance instances
        public async Task<IEnumerable<AttendanceInstance>> GetAllAttendanceAsync()
        {
            return await _context.AttendanceInstances.ToListAsync();
        }

        // Get attendance instances by class ID
        public async Task<IEnumerable<AttendanceInstance>> GetAttendanceByClassIdAsync(Guid classId)
        {
            return await _context.AttendanceInstances
                .Where(ai => ai.ClassId == classId)
                .ToListAsync();
        }

        // Get all excused absences
        public async Task<IEnumerable<AttendanceInstance>> GetExcusedAbsencesAsync(Guid? classId, string? date)
        {
            var query = _context.AttendanceInstances
                .Where(ai => ai.ExcusedAbsence == true);

            if (classId.HasValue)
            {
                query = query.Where(ai => ai.ClassId == classId.Value);
            }

            if (!string.IsNullOrWhiteSpace(date) && DateOnly.TryParse(date, out var parsedDate))
            {
                query = query.Where(ai =>
                    ai.DateTime.HasValue &&
                    DateOnly.FromDateTime(ai.DateTime.Value) == parsedDate);
            }

            return await query.ToListAsync();
        }

        // Get all lates
        public async Task<IEnumerable<AttendanceInstance>> GetLatesAsync(Guid? classId, string? date)
        {
            var query = _context.AttendanceInstances
                .Where(ai => ai.IsLate == true);

            if (classId.HasValue)
            {
                query = query.Where(ai => ai.ClassId == classId.Value);
            }

            if (!string.IsNullOrWhiteSpace(date) && DateOnly.TryParse(date, out var parsedDate))
            {
                query = query.Where(ai =>
                    ai.DateTime.HasValue &&
                    DateOnly.FromDateTime(ai.DateTime.Value) == parsedDate);
            }
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetAbsencesByDateAsync(Guid classId, string dateStr)
        {
            if (!DateOnly.TryParse(dateStr, out var parsedDate))
            {
                throw new ArgumentException("Invalid date format. Use YYYY-MM-DD.");
            }

            var enrolledStudents = await _context.StudentClasses
                .Where(sc => sc.ClassId == classId)
                .Select(sc => sc.Student)
                .ToListAsync();

            var presentStudentIds = await _context.AttendanceInstances
                .Where(ai =>
                    ai.ClassId == classId &&
                    ai.DateTime.HasValue &&
                    DateOnly.FromDateTime(ai.DateTime.Value) == parsedDate &&
                    ai.ExcusedAbsence == false)
                .Select(ai => ai.StudentId)
                .ToListAsync();

            var absentStudents = enrolledStudents
                .Where(s => !presentStudentIds.Contains(s.UtdId));

            return absentStudents;
        }

        // Get all attendance instances for a specific student
        public async Task<IEnumerable<AttendanceInstance>> GetAttendanceByStudentAsync(string studentId, string? date, Guid? classId)
        {
            var query = _context.AttendanceInstances
                .Where(ai => ai.StudentId == studentId);

            if (classId.HasValue)
            {
                query = query.Where(ai => ai.ClassId == classId.Value);
            }

            if (!string.IsNullOrWhiteSpace(date) && DateOnly.TryParse(date, out var parsedDate))
            {
                query = query.Where(ai =>
                    ai.DateTime.HasValue &&
                    DateOnly.FromDateTime(ai.DateTime.Value) == parsedDate);
            }

            return await query.ToListAsync();

        }

        // Add an attendance instance
        public async Task<AttendanceInstance> AddAttendanceInstanceAsync(AttendanceInstanceDto dto)
        {
            var attendance = new AttendanceInstance
            {
                AttendanceId = Guid.NewGuid(), // Auto-generate
                StudentId = dto.StudentId,
                ClassId = dto.ClassId,
                IpAddress = dto.IpAddress,

                // late students should be added from professor side
                IsLate = dto.IsLate, //null

                // excused absences should be input into database later from professor side
                ExcusedAbsence = dto.ExcusedAbsence, // null


                DateTime = dto.DateTime // UTC Now
            };

            _context.AttendanceInstances.Add(attendance);
            await _context.SaveChangesAsync();

            return attendance;
        }

        // Add an attendance instance for a student who was absent or late from the professor app
        public async Task<AttendanceInstance> AddAbsentOrLateAsync(AttendanceInstanceDto dto)
        {
            var newAttendance = new AttendanceInstance
            {
                AttendanceId = Guid.NewGuid(),
                StudentId = dto.StudentId,
                ClassId = dto.ClassId,
                IsLate = dto.IsLate ?? false, // either we set the IsLate to the value specified by the DTO, or we set it to false (in the case of this student being an absence and not late)
                ExcusedAbsence = dto.ExcusedAbsence ?? false, // either we set the ExcusedAbsence to the value specified by the DTO, or we set it to false (in the case of this student being late and not absent)
                DateTime = dto.DateTime ?? DateTime.Now,
                IpAddress = dto.IpAddress // professor app must pass in the IP address in the DTO
            };

            _context.AttendanceInstances.Add(newAttendance);
            await _context.SaveChangesAsync();

            return newAttendance;
        }
    }
}