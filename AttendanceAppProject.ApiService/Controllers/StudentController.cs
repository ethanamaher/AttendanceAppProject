﻿/* Student API Controller
 * Handles HTTP GET, POST, UPDATE, DELETE requests for students.
 * Written by Maaz Raza, Ethan Maher
 */

using AttendanceAppProject.ApiService.Services;
using AttendanceAppProject.ApiService.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AttendanceAppProject.Dto.Models;

namespace AttendanceAppProject.ApiService.Controllers
{
    [Route("api/[controller]")] // Automatically becomes "api/student"
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentService _service;

        public StudentController(StudentService service)
        {
            _service = service;
        }

        /* GET: api/Student
         * Get all students from the database
         * - request body: none
         * - response body: students
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return Ok(await _service.GetStudentsAsync());
        }
        /* POST: api/Student
         * Add a student to the database
         * - request body: StudentDto
         * - response body: Student
         */
        [HttpPost]
        public async Task<ActionResult<Student>> AddStudent([FromBody] StudentDto dto)
        {
            var student = await _service.AddStudentAsync(dto);

            return CreatedAtAction(nameof(GetStudents), new { id = student.UtdId }, student);
        }

        /* POST: api/student/exists
         * Check if a student exists in the database by validating the UtdId of a given StudentDto passed in by the client side
         * - request body: String utdId
         * - response body: HttpResponse
         */
        [HttpPost("exists")]
        public async Task<ActionResult<bool>> StudentExists([FromBody] String UtdId)
        {
			if (string.IsNullOrWhiteSpace(UtdId))
            {
                return BadRequest("UtdId is required."); // 400
            }

            var exists = await _service.StudentExistsAsync(UtdId);
            if (!exists)
            {
                return NotFound($"Student with ID {UtdId} not found"); // 404
            }
            return Ok(exists); // 200
        }

        /* PUT: api/Student/{utdId}
         * Update a student by UtdId
         * - request body: StudentDto
         * - response body: Student
         */
        [HttpPut("{utdId}")]
        public async Task<ActionResult<Student>> UpdateStudent(string utdId, [FromBody] StudentDto updatedStudent)
        {
            var student = await _service.UpdateStudentAsync(utdId, updatedStudent);
            if (student == null)
                return NotFound();

            return Ok(student);
        }

        /* DELETE: api/Student/{utdId}
         * Delete a student by UtdId
         * - request body: none
         * - response body: HttpResponse
         */
        [HttpDelete("{utdId}")]
        public async Task<IActionResult> DeleteStudent(string utdId)
        {
            var success = await _service.DeleteStudentAsync(utdId);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}