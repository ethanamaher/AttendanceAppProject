using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.ApiService.Data.Models;
using Microsoft.EntityFrameworkCore;
using AttendanceAppProject.Dto.Models;

namespace AttendanceAppProject.ApiService.Services
{
    public class StudentService
    {
        private readonly ApplicationDbContext _context;

        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetStudentsAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> AddStudentAsync(StudentDto dto)
        {
            var student = new Student
            {
                UtdId = dto.UtdId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Username = dto.Username
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<bool> StudentExistsAsync(String UtdId)
        {
            return await _context.Students.AnyAsync(s => s.UtdId == UtdId);
        }

        // Update a student by UtdId
        public async Task<Student?> UpdateStudentAsync(string utdId, StudentDto updatedStudent)
        {
            var student = await _context.Students.FindAsync(utdId);
            if (student == null)
            {
                return null;
            }

            student.FirstName = updatedStudent.FirstName ?? student.FirstName;
            student.LastName = updatedStudent.LastName ?? student.LastName;
            student.Username = updatedStudent.Username ?? student.Username;

            await _context.SaveChangesAsync();
            return student;
        }

        // Delete a student by UtdId
        public async Task<bool> DeleteStudentAsync(string utdId)
        {
            var student = await _context.Students.FindAsync(utdId);
            if (student == null)
            {
                return false;
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
