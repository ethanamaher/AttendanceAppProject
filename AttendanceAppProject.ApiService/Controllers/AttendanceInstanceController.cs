/* AttendanceInstance API Controller
 * Handles HTTP GET and POST requests for attendance instances, allowing for retrieval and creation of attendance records.
 * Written by Maaz Raza 
 */

using Microsoft.AspNetCore.Mvc;
using AttendanceAppProject.ApiService.Data.Models;
using Microsoft.EntityFrameworkCore;
using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.Dto.Models;
using AttendanceAppProject.ApiService.Services;

namespace AttendanceAppProject.ApiService.Controllers
{
    [Route("api/[controller]")] // Automatically becomes "api/AttendanceInstance"
    [ApiController]
    public class AttendanceInstanceController : ControllerBase
    {
        private readonly AttendanceInstanceService _service;

        private readonly ApplicationDbContext _context;


        // Dependency injection - allows ASP.NET Core to pass an instance of ApplicationDbContext into the controller's constructor whenever the API is called so it can interact with the db
        public AttendanceInstanceController(AttendanceInstanceService service, ApplicationDbContext context)
        {
            _service = service;
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
            return Ok(await _service.GetAllAttendanceAsync());
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
            var attendanceList = await _service.GetAttendanceByClassIdAsync(classId);
               
            if (attendanceList == null)
            {
                return NotFound();
            }

            return Ok(attendanceList);
        }

        /* GET: api/AttendanceInstance/excused-absences/?classId=...&date=...
         * Get all attendance instances that are marked as excused absences. Optionally include class ID and date to filter out the results.
         * Date in format YYYY-MM-DD
         * - request body: none
         * - response body: List of AttendanceInstances
         */
        [HttpGet("excused-absences")]
        public async Task<ActionResult<IEnumerable<AttendanceInstance>>> GetExcusedAbsences([FromQuery] Guid? classId,
    [FromQuery] string? date)
        {
            
            var absences = await _service.GetExcusedAbsences(classId, date);

            if (absences == null)
            {
                return NotFound();
            }
            return Ok(absences);
        }

        /* GET: api/AttendanceInstance/lates?classId=...&date=...
         * Get all attendance instances that are marked as late. Optionally include class ID and date to filter out the results.
         * Date in format YYYY-MM-DD
         * - request body: none
         * - response body: List of AttendanceInstances
         */
        [HttpGet("lates")]
        public async Task<ActionResult<IEnumerable<AttendanceInstance>>> GetLates(
            [FromQuery] Guid? classId,
            [FromQuery] string? date)
        {
            var lates = await _service.GetLates(classId, date);

            if (lates == null)
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

            var absentStudents = await _service.GetAbsentStudentsByDate(classId, dateStr);

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
            var records = _service.GetAttendanceByStudent(studentId, date, classId);

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

                // late students should be added from professor side
                IsLate = dto.IsLate, //null

                // excused absences should be input into database later from professor side
                ExcusedAbsence = dto.ExcusedAbsence, // null


                DateTime = dto.DateTime // UTC Now
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
                IpAddress = dto.IpAddress // professor app must pass in the IP address in the DTO
            };

            _context.AttendanceInstances.Add(newAttendance);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAttendanceInstances), new { id = newAttendance.AttendanceId }, newAttendance);
        }
    }
}
