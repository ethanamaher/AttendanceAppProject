/* API Service for Professor - written to handle class-related operations and delegate logic separate to API controller for better organization
 * Written by Maaz Raza
 */

using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.ApiService.Data.Models;
using AttendanceAppProject.Dto.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceAppProject.ApiService.Services
{
    public class ProfessorService
    {
        private readonly ApplicationDbContext _context;

        public ProfessorService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all professors
        public async Task<IEnumerable<Professor>> GetProfessorsAsync()
        {
            return await _context.Professors.ToListAsync();
        }

        public async Task<Professor> AddProfessorAsync(ProfessorDto dto)
        {
            var professor = new Professor
            {
                UtdId = dto.UtdId,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            _context.Professors.Add(professor);
            await _context.SaveChangesAsync();
            return professor;
        }
    }
}
