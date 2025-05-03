/* StudentClass API Controller
 * Handles HTTP GET and POST requests for StudentClass, allowing for retrieval and creation of StudentClass record which map students to classes, and to verify if a student is enrolled in a particular class.
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
    [Route("api/[controller]")] // Automatically becomes "api/StudentClass" (case-insensitive)
    [ApiController]
    public class StudentClassController : ControllerBase
    {
        private readonly StudentClassService _service;

        // Constructor to initialize the StudentClassService
        public StudentClassController(StudentClassService service)
        {
            _service = service;
        }

        /* GET: api/StudentClass
         * Get all StudentClass records
         * - request body: none
         * - response body: <IEnumerable<StudentClass>>
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentClass>>> GetStudentClasses()
        {
            return Ok(await _service.GetStudentClassesAsync());
        }

        /* POST: api/StudentClass
         * Add a StudentClass record to the database
         * - request body: StudentClassDto
         * - response body: StudentClass
         */
        [HttpPost]
        public async Task<ActionResult<StudentClass>> AddStudentClass([FromBody] StudentClassDto dto)
        {
            var studentClass = await _service.AddStudentClassAsync(dto);

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
            var exists = await _service.CheckEnrollmentAsync(dto);

            return Ok(exists); // true if enrolled, false otherwise
        }

        /* PUT: api/StudentClass/{studentClassId}
         * Update a student-class enrollment by StudentClassId
         * - request body: StudentClassDto
         * - response body: StudentClass
         */
        [HttpPut("{studentClassId}")]
        public async Task<ActionResult<StudentClass>> UpdateStudentClass(Guid studentClassId, [FromBody] StudentClassDto updatedStudentClass)
        {
            var result = await _service.UpdateStudentClassAsync(studentClassId, updatedStudentClass);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /* DELETE: api/StudentClass/{studentClassId}
         * Delete a student-class enrollment by StudentClassId
         * - request body: none
         * - response body: HttpResponse
         */
        [HttpDelete("{studentClassId}")]
        public async Task<IActionResult> DeleteStudentClass(Guid studentClassId)
        {
            var success = await _service.DeleteStudentClassAsync(studentClassId);
            if (!success)
                return NotFound();

            return NoContent();
        }

    }
}



       
         
         