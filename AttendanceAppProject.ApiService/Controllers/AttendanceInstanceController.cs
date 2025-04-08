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
    }
}
