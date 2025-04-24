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

        // Get students who missed X consecutive class meetings (days the class is scheduled to meet) for a specific class
        public async Task<IEnumerable<StudentDto>> GetStudentsWithConsecutiveAbsencesAsync(Guid classId, int consecutiveMisses)
        {
            // Retrieve class details including schedule and enrolled students
            var classInfo = await _context.Classes
                .Include(c => c.ClassSchedules)
                .Include(c => c.StudentClasses)
                    .ThenInclude(sc => sc.Student)
                .FirstOrDefaultAsync(c => c.ClassId == classId);

            if (classInfo == null)
                return Enumerable.Empty<StudentDto>();

            // Days of the week when class meets (e.g., Monday, Wednesday)
            var scheduledDaysOfWeek = classInfo.ClassSchedules.Select(cs => Enum.Parse<DayOfWeek>(cs.DayOfWeek)).ToList();

            // Generate all scheduled class dates within start and end dates
            var allClassDates = Enumerable.Range(0, (classInfo.EndDate.Value.ToDateTime(TimeOnly.MinValue) - classInfo.StartDate.Value.ToDateTime(TimeOnly.MinValue)).Days + 1)
                .Select(offset => classInfo.StartDate.Value.AddDays(offset))
                .Where(date => scheduledDaysOfWeek.Contains(date.DayOfWeek))
                .ToList();

            var absentStudents = new List<StudentDto>();

            foreach (var studentClass in classInfo.StudentClasses)
            {
                int consecutiveAbsences = 0;
                bool hasMetConsecutiveRequirement = false;

                foreach (var classDate in allClassDates)
                {
                    // Check if attendance instance exists for the student on this date and is not excused
                    bool attended = await _context.AttendanceInstances.AnyAsync(ai =>
                        ai.ClassId == classId &&
                        ai.StudentId == studentClass.StudentId &&
                        ai.DateTime.HasValue &&
                        DateOnly.FromDateTime(ai.DateTime.Value) == classDate);

                    if (!attended)
                        consecutiveAbsences++;
                    else
                        consecutiveAbsences = 0; // reset if student attended

                    if (consecutiveAbsences >= consecutiveMisses)
                    {
                        hasMetConsecutiveRequirement = true;
                        break; // no need to check further for this student
                    }
                }

                if (hasMetConsecutiveRequirement)
                {
                    absentStudents.Add(new StudentDto
                    {
                        UtdId = studentClass.Student.UtdId,
                        FirstName = studentClass.Student.FirstName,
                        LastName = studentClass.Student.LastName,
                        Username = studentClass.Student.Username
                    });
                }
            }

            return absentStudents;
        }

        // Get students who missed X total class meetings (days the class is scheduled to meet) for a specific class
        public async Task<IEnumerable<StudentDto>> GetStudentsWhoMissedXClassesAsync(Guid classId, int missedClassesCount)
        {
            // Retrieve class details including schedule and enrolled students
            var classInfo = await _context.Classes
                .Include(c => c.ClassSchedules)
                .Include(c => c.StudentClasses)
                    .ThenInclude(sc => sc.Student)
                .FirstOrDefaultAsync(c => c.ClassId == classId);

            if (classInfo == null)
                return Enumerable.Empty<StudentDto>();

            // Days of the week when class meets
            var scheduledDaysOfWeek = classInfo.ClassSchedules.Select(cs => Enum.Parse<DayOfWeek>(cs.DayOfWeek)).ToList();

            // Generate all scheduled class dates within start and end dates
            var allClassDates = Enumerable.Range(0, (classInfo.EndDate.Value.ToDateTime(TimeOnly.MinValue) - classInfo.StartDate.Value.ToDateTime(TimeOnly.MinValue)).Days + 1)
                .Select(offset => classInfo.StartDate.Value.AddDays(offset))
                .Where(date => scheduledDaysOfWeek.Contains(date.DayOfWeek))
                .ToList();

            var absentStudents = new List<StudentDto>();

            foreach (var studentClass in classInfo.StudentClasses)
            {
                int totalAbsences = 0;

                foreach (var classDate in allClassDates)
                {
                    // Check if attendance instance exists for the student on this date and is not excused
                    bool attended = await _context.AttendanceInstances.AnyAsync(ai =>
                        ai.ClassId == classId &&
                        ai.StudentId == studentClass.StudentId &&
                        ai.DateTime.HasValue &&
                        DateOnly.FromDateTime(ai.DateTime.Value) == classDate);

                    if (!attended)
                        totalAbsences++;
                }

                if (totalAbsences >= missedClassesCount)
                {
                    absentStudents.Add(new StudentDto
                    {
                        UtdId = studentClass.Student.UtdId,
                        FirstName = studentClass.Student.FirstName,
                        LastName = studentClass.Student.LastName,
                        Username = studentClass.Student.Username
                    });
                }
            }

            return absentStudents;
        }
    }
}