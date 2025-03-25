using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.ApiService.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

		// GET: api/Student
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Class>>> GetClasses()
		{
			return await _context.Classes.ToListAsync();
		}
	}
}
