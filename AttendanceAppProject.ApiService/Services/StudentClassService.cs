using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.ApiService.Data.Models;
using Microsoft.EntityFrameworkCore;
using AttendanceAppProject.Dto.Models;

namespace AttendanceAppProject.ApiService.Services
{
    public class StudentClassService
    {
        private readonly ApplicationDbContext _context;

        public StudentClassService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudentClass>> GetStudentClassesAsync()
        {
            return await _context.StudentClasses.ToListAsync();
        }

        public async Task<StudentClass> AddStudentClassAsync(StudentClassDto dto)
        {
            var studentClass = new StudentClass
            {
                StudentClassId = Guid.NewGuid(), // Auto-generate
                StudentId = dto.StudentId,
                ClassId = dto.ClassId
            };

            _context.StudentClasses.Add(studentClass);
            await _context.SaveChangesAsync();
            return studentClass;
        }

        public async Task<bool> CheckEnrollmentAsync(EnrollmentCheckDto dto)
        {
            var exists = await _context.StudentClasses
                .AnyAsync(sc => sc.StudentId == dto.Student.UtdId && sc.ClassId == dto.Class.ClassId); // sc is StudentClass object within the database
            return exists;
        }

        // Update a student-class enrollment by StudentClassId
        public async Task<StudentClass?> UpdateStudentClassAsync(Guid studentClassId, StudentClassDto updatedStudentClass)
        {
            var existingEnrollment = await _context.StudentClasses.FindAsync(studentClassId);
            if (existingEnrollment == null)
            {
                return null;
            }

            existingEnrollment.StudentId = updatedStudentClass.StudentId ?? existingEnrollment.StudentId;
            existingEnrollment.ClassId = updatedStudentClass.ClassId != Guid.Empty ? updatedStudentClass.ClassId : existingEnrollment.ClassId;

            await _context.SaveChangesAsync();
            return existingEnrollment;
        }

        // Delete a student-class enrollment by StudentClassId
        public async Task<bool> DeleteStudentClassAsync(Guid studentClassId)
        {
            var enrollment = await _context.StudentClasses.FindAsync(studentClassId);
            if (enrollment == null)
            {
                return false;
            }

            _context.StudentClasses.Remove(enrollment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
