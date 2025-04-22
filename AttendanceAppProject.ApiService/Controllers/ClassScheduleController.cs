/* Class API Controller
 * Handles HTTP GET and POST requests for classes, allowing for retrieval and creation of class records, retrieving a class by its ID, and checking if a class exists in the database.
 * Written by Ethan Maher, Maaz Raza
 */

using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.ApiService.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AttendanceAppProject.Dto.Models;

// API controller for Class

namespace AttendanceAppProject.ApiService.Controllers
{
    [Route("api/[controller]")] // Automatically becomes "api/classschedule"
    [ApiController]
    public class ClassScheduleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClassScheduleController(ApplicationDbContext context)
        {
            _context = context;
        }

        /* GET: api/classschedule
		 * Get all classschedules
		 * - request body: none
		 * - response body: IEnumerable<ClassSchedule>
		 */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassSchedule>>> GetAllClassSchedules()
        {
            return await _context.ClassSchedules.ToListAsync();
        }

        /* GET: api/class/{id}
		 * Get List of classscheduleitems whose classId private key = id
		 * - request body: Guid classId
		 * - response body: IEnumerable<ClassSchedule>
		 */
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ClassSchedule>>> GetClassSchedule(Guid id)
        {
            var scheduleItem = await _context.ClassSchedules.Where(c => c.ClassId == id).ToListAsync();

            if (scheduleItem == null)
            {
                return NotFound();
            }

            return scheduleItem;
        }
    }
}
