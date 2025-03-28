using Microsoft.AspNetCore.Mvc;
using AttendanceAppProject.ApiService.Data.Models;
using Microsoft.EntityFrameworkCore;
using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.Dto.Models;

namespace AttendanceAppProject.ApiService.Controllers
{
    [Route("api/[controller]")] // Automatically becomes "api/studentclass"
    [ApiController]
    public class StudentClassController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentClassController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/StudentClass
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentClass>>> GetStudentClasses()
        {
            return await _context.StudentClasses.ToListAsync();
        }

        // POST: api/StudentClass
        [HttpPost]
        public async Task<ActionResult<Student>> AddStudentClass([FromBody] StudentClassDto dto)
        {
            var studentClass = new StudentClass
            {
                StudentClassId = Guid.NewGuid(), // Auto-generate
                StudentId = dto.StudentId,
                ClassId = dto.ClassId
            };

            _context.StudentClasses.Add(studentClass);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudentClasses), new { id = studentClass.StudentClassId }, studentClass);
        }

    }
}
