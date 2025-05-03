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

        // Update a password by class ID
        public async Task<Password?> UpdatePasswordAsync(Guid classId, PasswordDto updatedPassword)
        {
            var password = await _context.Passwords.FirstOrDefaultAsync(p =>
                p.ClassId == classId
            );

            System.Diagnostics.Debug.WriteLine($"Found Entry For: {classId}");
            if (password == null)
            {
                return null;
            }

            password.PasswordText = updatedPassword.PasswordText ?? password.PasswordText;
            password.DateAssigned = updatedPassword.DateAssigned ?? password.DateAssigned;
            // password.ClassId = updatedPassword.ClassId;

            await _context.SaveChangesAsync();
            return password;
        }

        // Delete a password by ID
        public async Task<bool> DeletePasswordAsync(Guid id)
        {
            var password = await _context.Passwords.FindAsync(id);
            if (password == null)
            {
                return false;
            }

            _context.Passwords.Remove(password);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
