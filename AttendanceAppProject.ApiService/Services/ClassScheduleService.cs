using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.ApiService.Data.Models;
using AttendanceAppProject.Dto.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceAppProject.ApiService.Services
{
    public class ClassScheduleService
    {
        private readonly ApplicationDbContext _context;

        public ClassScheduleService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all class schedules
        public async Task<IEnumerable<ClassSchedule>> GetClassSchedulesAsync()
        {
            return await _context.ClassSchedules.ToListAsync();
        }

        // Get class schedule by id
        public async Task<IEnumerable<ClassSchedule>> GetClassSchedulesByIdAsync(Guid id)
        {
            return await _context.ClassSchedules.Where(c => c.ClassId == id).ToListAsync();
        }
    }
}
