/* AttendanceInstance API Controller
 * Handles HTTP GET and POST requests for attendance instances, allowing for retrieval and creation of attendance records.
 * Written by Maaz Raza 
 */

using Microsoft.AspNetCore.Mvc;
using AttendanceAppProject.ApiService.Data.Models;
using Microsoft.EntityFrameworkCore;
using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.Dto.Models;

namespace AttendanceAppProject.ApiService.Controllers
{
    [Route("api/[controller]")] // Automatically becomes "api/AttendanceInstance"
    [ApiController]
    public class AttendanceInstanceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Dependency injection - allows ASP.NET Core to pass an instance of ApplicationDbContext into the controller's constructor whenever the API is called so it can interact with the db
        public AttendanceInstanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        /* GET: api/AttendanceInstance
         * Get all attendance instances from the database
         * - request body: none
         * - response body: AttendanceInstances
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AttendanceInstance>>> GetAttendanceInstances()
        {
            return await _context.AttendanceInstances.ToListAsync();
        }

        /* GET: api/AttendanceInstance/class/{classId}
         * Get all attendance instances for a particular class, used by professor
         * - request body: none
         * - response body: List of AttendanceInstances
         * example usage: attendanceList = await Http.GetFromJsonAsync<List<AttendanceInstanceDto>>($"api/AttendanceInstance/class/{selectedClassId}"
         */
        [HttpGet("class/{classId}")]
        public async Task<ActionResult<IEnumerable<AttendanceInstance>>> GetByClassId(Guid classId)
        {
            var attendanceList = await _context.AttendanceInstances
                .Where(ai => ai.ClassId == classId)
                .ToListAsync();
            if (attendanceList == null || attendanceList.Count == 0)
            {
                return NotFound();
            }

            return Ok(attendanceList);
        }

        /* GET: api/AttendanceInstance/excused-absences/?classId=...&date=...
         * Get all attendance instances that are marked as excused absences. Optionally include class ID and date to filter out the results.
         * - request body: none
         * - response body: List of AttendanceInstances
         */
        [HttpGet("excused-absences")]
        public async Task<ActionResult<IEnumerable<AttendanceInstance>>> GetExcusedAbsences([FromQuery] Guid? classId,
    [FromQuery] string? date)
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

            var absences = await query.ToListAsync();

            if (absences.Count == 0)
            {
                return NotFound();
            }
            return Ok(absences);
        }

        /* GET: api/AttendanceInstance/lates?classId=...&date=...
         * Get all attendance instances that are marked as late. Optionally include class ID and date to filter out the results.
         * - request body: none
         * - response body: List of AttendanceInstances
         */
        [HttpGet("lates")]
        public async Task<ActionResult<IEnumerable<AttendanceInstance>>> GetLates(
            [FromQuery] Guid? classId,
            [FromQuery] string? date)
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

            var lates = await query.ToListAsync();

            if (lates.Count == 0)
            {
                return NotFound();
            }

            return Ok(lates);
        }

        /* GET: api/AttendanceInstance/class/{classId}/absent-on-date/{dateStr}
         * Get the names of all students who were absent on a particular date for a given class, to be used by the professor
         * - request body: none
         * - response body: List of DTOs of students who were absent on the given date
         */
        [HttpGet("class/{classId}/absent-on-date/{dateStr}")]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetAbsentStudentsByDate(Guid classId, string dateStr)
        {
            if (!DateOnly.TryParse(dateStr, out var date))
            {
                return BadRequest("Invalid date format. Use YYYY-MM-DD.");
            }

            // Get all students enrolled in this class
            var enrolledStudents = await _context.StudentClasses
                .Where(sc => sc.ClassId == classId)
                .Select(sc => sc.Student)
                .ToListAsync();

            // Get student IDs with attendance on this date (ignoring excused absences but including late)
            var presentStudentIds = await _context.AttendanceInstances
                .Where(ai => // ai = attendance instance
                    ai.ClassId == classId &&
                    ai.DateTime.HasValue &&
                    DateOnly.FromDateTime(ai.DateTime.Value) == date &&
                    ai.ExcusedAbsence == false)
                .Select(ai => ai.StudentId)
                .ToListAsync();

            // Filter out those students who were present on this date
            var absentStudents = enrolledStudents
                .Where(s => !presentStudentIds.Contains(s.UtdId))
                .ToList();

            return Ok(absentStudents);
        }

        /* GET: api/AttendanceInstance/student/{studentId}?date=2025-02-01&classId=...
         * Get all attendance instances for a specific student.
         * Optionally filter by date (YYYY-MM-DD format).
         * - request body: none
         * - response body: List of AttendanceInstances
         */
        [HttpGet("student/{studentId}")]
        public async Task<ActionResult<IEnumerable<AttendanceInstance>>> GetAttendanceByStudent(
            string studentId,
            [FromQuery] string? date,
            [FromQuery] Guid? classId)
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

            var records = await query.ToListAsync();

            if (records.Count == 0)
            {
                return NotFound();
            }

            return Ok(records);
        }

        /* POST: api/AttendanceInstance
         * Add an attendance instance to the database
         * - request body: AttendanceInstanceDto
         * - response body: AttendanceInstance
         */
        [HttpPost]
        public async Task<ActionResult<AttendanceInstance>> AddAttendanceInstance([FromBody] AttendanceInstanceDto dto)
        {
            var attendance = new AttendanceInstance
            {
                AttendanceId = Guid.NewGuid(), // Auto-generate
                StudentId = dto.StudentId,
                ClassId = dto.ClassId,
                IpAddress = dto.IpAddress,
                IsLate = dto.IsLate,
                ExcusedAbsence = dto.ExcusedAbsence,
                DateTime = dto.DateTime
            };

            _context.AttendanceInstances.Add(attendance);
            await _context.SaveChangesAsync();

            // this is the conventional way, to return HTTP 201 created code and a Location header pointing to where the new resource can be found, and the new resource itself in the response body 
            return CreatedAtAction(nameof(GetAttendanceInstances), new { id = attendance.AttendanceId }, attendance);
        }

        /* POST: api/AttendanceInstance/absent-or-late
         * Add an attendnace instance to the database for students who were absent or late, entered on the client side by the professor
         * - request body: AttendanceInstanceDto (must have either IsLate or IsAbsent set to true)
         * - response body: AttendanceInstance 
         */
        [HttpPost("absent-or-late")]
        public async Task<ActionResult<AttendanceInstance>> AddAbsentOrLate([FromBody] AttendanceInstanceDto dto)
        {
            var newAttendance = new AttendanceInstance
            {
                AttendanceId = Guid.NewGuid(),
                StudentId = dto.StudentId,
                ClassId = dto.ClassId,
                IsLate = dto.IsLate ?? false, // either we set the IsLate to the value specified by the DTO, or we set it to false (in the case of this student being an absence and not late)
                ExcusedAbsence = dto.ExcusedAbsence ?? false, // either we set the ExcusedAbsence to the value specified by the DTO, or we set it to false (in the case of this student being late and not absent)
                DateTime = dto.DateTime ?? DateTime.Now,
                IpAddress = "127.0.0.1" // since it is manually entered by professor we will use a placeholder IP
            };

            _context.AttendanceInstances.Add(newAttendance);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAttendanceInstances), new { id = newAttendance.AttendanceId }, newAttendance);
        }
    }
}
