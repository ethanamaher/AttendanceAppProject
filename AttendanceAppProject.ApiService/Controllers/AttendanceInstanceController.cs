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
