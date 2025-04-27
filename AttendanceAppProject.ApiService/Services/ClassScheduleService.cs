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

        // Update a class schedule by ID
        public async Task<ClassSchedule?> UpdateClassScheduleAsync(Guid id, ClassScheduleDto updatedSchedule)
        {
            var schedule = await _context.ClassSchedules.FindAsync(id);
            if (schedule == null)
            {
                return null;
            }

            schedule.DayOfWeek = updatedSchedule.DayOfWeek ?? schedule.DayOfWeek;
            schedule.ClassId = updatedSchedule.ClassId;

            await _context.SaveChangesAsync();
            return schedule;
        }

        // Delete a class schedule by ID
        public async Task<bool> DeleteClassScheduleAsync(Guid id)
        {
            var schedule = await _context.ClassSchedules.FindAsync(id);
            if (schedule == null)
            {
                return false;
            }

            _context.ClassSchedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
