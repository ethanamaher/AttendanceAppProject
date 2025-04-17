/* Class API Controller
 * Handles HTTP GET and POST requests for classes, allowing for retrieval and creation of class records, retrieving a class by its ID, and checking if a class exists in the database.
 * Written by Ethan Maher, Maaz Raza
 */

using AttendanceAppProject.ApiService.Services;
using AttendanceAppProject.ApiService.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AttendanceAppProject.Dto.Models;

// API controller for ClassSchedule

namespace AttendanceAppProject.ApiService.Controllers
{
    [Route("api/[controller]")] // Automatically becomes "api/classschedule"
    [ApiController]
    public class ClassScheduleController : ControllerBase
    {
        private readonly ClassScheduleService _service;

        public ClassScheduleController(ClassScheduleService service)
        {
            _service = service;
        }

        /* GET: api/classschedule
		 * Get all classschedules
		 * - request body: none
		 * - response body: IEnumerable<ClassSchedule>
		 */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassSchedule>>> GetAllClassSchedules()
        {
            return Ok(await _service.GetClassSchedulesAsync());
        }

        /* GET: api/classschedule/{id}
		 * Get List of classscheduleitems whose classId private key = id
		 * - request body: Guid classId
		 * - response body: IEnumerable<ClassSchedule>
		 */
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ClassSchedule>>> GetClassSchedule(Guid id)
        {
            var scheduleItem = await _service.GetClassSchedulesByIdAsync(id);
            if (scheduleItem == null)
                return NotFound();

            return Ok(scheduleItem);
        }
    }
}
