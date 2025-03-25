using Microsoft.AspNetCore.Mvc;
using AttendanceAppProject.ApiService.Data.Models;
using Microsoft.EntityFrameworkCore;
using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.Dto.Models;

namespace AttendanceAppProject.ApiService.Controllers
{
	[Route("api/[controller]")] // Automatically becomes "api/class"
	[ApiController]
	public class ClassController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public ClassController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: api/Class
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Class>>> GetClasses()
		{
			return await _context.Classes.ToListAsync();
		}

        // POST: api/Class
        [HttpPost]
        public async Task<ActionResult<Class>> AddClass([FromBody] ClassDto dto)
        {
            var newClass = new Class
            {
                ClassId = Guid.NewGuid(), // Auto-generate
                ProfUtdId = dto.ProfUtdId,
                ClassPrefix = dto.ClassPrefix,
                ClassNumber = dto.ClassNumber,
                ClassName = dto.ClassName,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime
            };

            _context.Classes.Add(newClass);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClasses), new { id = newClass.ClassId }, newClass);
        }
    }
}
