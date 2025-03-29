using Microsoft.AspNetCore.Mvc;
using AttendanceAppProject.ApiService.Data.Models;
using Microsoft.EntityFrameworkCore;
using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.Dto.Models;

// API controller for StudentClass relation

namespace AttendanceAppProject.ApiService.Controllers
{
    [Route("api/[controller]")] // Automatically becomes "api/StudentClass" (case-insensitive)
    [ApiController]
    public class StudentClassController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentClassController(ApplicationDbContext context)
        {
            _context = context;
        }

        /* GET: api/StudentClass
         * Get all StudentClass records
         * - request body: none
         * - response body: StudentClasses
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentClass>>> GetStudentClasses()
        {
            return await _context.StudentClasses.ToListAsync();
        }

        /* POST: api/StudentClass
         * Add a StudentClass record to the database
         * - request body: StudentClassDto
         * - response body: none
         */
        [HttpPost]
        public async Task<ActionResult<StudentClass>> AddStudentClass([FromBody] StudentClassDto dto)
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

        /* POST: api/StudentClass/check-enrollment
         * Check if a student is enrolled in a particular class using EnrollmentCheckDto which is a wrapper DTO that contains StudentDto and ClassDto.
         * The client side encapsulates student DTO and class DTO into a single DTO (EnrollmentCheckDto), sends a POST request to the server and the server side uses that EnrollmentCheckDto validate the IDs from there
         * - request body: EnrollmentCheckDto 
         * - response body: true if enrolled, false otherwise
            Example usage on how to send the request on the client side:
                var checkDto = new EnrollmentCheckDto
                {
                    Student = studentDto,
                    Class = classDto
                };
                await Http.PostAsJsonAsync("api/studentclass/check-enrollment", checkDto); 
         */
        [HttpPost("check-enrollment")]
        public async Task<ActionResult<bool>> CheckEnrollment([FromBody] EnrollmentCheckDto dto)
        {
            var exists = await _context.StudentClasses
                .AnyAsync(sc => sc.StudentId == dto.Student.UtdId && sc.ClassId == dto.Class.ClassId); // sc is StudentClass object within the database

            return Ok(exists); // true if enrolled, false otherwise
        }

    }
}



       
         
         