/* API Service for Class - written to handle class-related operations and delegate logic separate to API controller for better organization
 * Written by Ethan Maher, Maaz Raza
 */

using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.ApiService.Data.Models;
using AttendanceAppProject.Dto.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceAppProject.ApiService.Services
{
    public class ClassService
    {
        private readonly ApplicationDbContext _context;
        private readonly ServiceProvider _serviceProvider;
        

        public ClassService(ServiceProvider serviceProvider, ApplicationDbContext context)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        // Get all classes
        public async Task<IEnumerable<Class>> GetAllClassesAsync()
        {
            return await _context.Classes.ToListAsync();
        }

        // Get class by ID
        public async Task<Class?> GetClassByIdAsync(Guid id)
        {
            return await _context.Classes.FirstOrDefaultAsync(c => c.ClassId == id);
        }

        // Get classes by professor ID
        public async Task<IEnumerable<Class>> GetClassesByProfessorIdAsync(string profUtdId)
        {
            return await _context.Classes
                .Where(c => c.ProfUtdId == profUtdId)
                .ToListAsync();
        }

        // Check if class exists by ID
        public async Task<bool> ClassExistsAsync(Guid classId)
        {
            return await _context.Classes.AnyAsync(c => c.ClassId == classId);
        }

        // Add a new class
        public async Task<Class> AddClassAsync(ClassDto dto)
        {
            var newClass = new Class
            {
                ClassId = Guid.NewGuid(),
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

            return newClass;
        }
    }
}