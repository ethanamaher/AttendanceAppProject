/* API Service for Password - written to handle class-related operations and delegate logic separate to API controller for better organization
 * Written by Maaz Raza
 */

using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.ApiService.Data.Models;
using AttendanceAppProject.Dto.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceAppProject.ApiService.Services
{
    public class PasswordService
    {
        private readonly ApplicationDbContext _context;

        public PasswordService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all passwords
        public async Task<IEnumerable<Password>> GetPasswordsAsync()
        {
            return await _context.Passwords.ToListAsync();
        }

        // Add a password to the database
        public async Task<Password> AddPasswordAsync(PasswordDto dto)
        {
            var password = new Password
            {
                PasswordId = Guid.NewGuid(), // Auto-generate
                ClassId = dto.ClassId,
                PasswordText = dto.PasswordText,
                DateAssigned = dto.DateAssigned ?? DateOnly.FromDateTime(DateTime.Now)
            };
            _context.Passwords.Add(password);
            await _context.SaveChangesAsync();
            return password;
        }

        // Validate that a password exists in the database
        public async Task<bool> ValidatePasswordAsync(PasswordDto dto)
        {
            var exists = await _context.Passwords.AnyAsync(p =>
                p.ClassId == dto.ClassId &&
                p.PasswordText.ToLower() == dto.PasswordText.ToLower() &&
                p.DateAssigned == dto.DateAssigned
            );
            return exists;
        }
    }
}
