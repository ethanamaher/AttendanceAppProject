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

        public AttendanceInstanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/AttendanceInstance
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AttendanceInstance>>> GetAttendanceInstances()
        {
            return await _context.AttendanceInstances.ToListAsync();
        }

        // POST: api/AttendanceInstance
        [HttpPost]
        public async Task<ActionResult<AttendanceInstance>> AddAttendanceInstance([FromBody] AttendanceInstanceDto dto)
        {
            var attendance = new AttendanceInstance
            {
                AttendanceId = dto.AttendanceId,
                StudentId = dto.StudentId,
                ClassId = dto.ClassId,
                IpAddress = dto.IpAddress,
                IsLate = dto.IsLate,
                ExcusedAbsence = dto.ExcusedAbsence,
                DateTime = dto.DateTime
            };

            _context.AttendanceInstances.Add(attendance);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAttendanceInstances), new { id = attendance.AttendanceId }, attendance);
        }
    }
}
