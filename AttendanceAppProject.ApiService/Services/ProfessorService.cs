﻿/* Updated API Service for Professor with authentication
 * Written to handle professor-related operations and authentication.
 * Written by Canh Nguyen, Maaz Raza
 */

using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.ApiService.Data.Models;
using AttendanceAppProject.Dto.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceAppProject.ApiService.Services
{
    public class ProfessorService
    {
        private readonly ApplicationDbContext _context;

        // Constructor to initialize the ApplicationDbContext
        public ProfessorService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all professors
        public async Task<IEnumerable<Professor>> GetProfessorsAsync()
        {
            return await _context.Professors.ToListAsync();
        }

        public async Task<Professor?> GetProfessorByIdAsync(String UtdId)
        {
            return await _context.Professors.FirstOrDefaultAsync(p => p.UtdId == UtdId);
        }

        // Add a professor
        public async Task<Professor> AddProfessorAsync(ProfessorDto dto)
        {
            var professor = new Professor
            {
                UtdId = dto.UtdId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PasswordHash = CreatePasswordHash(dto.Password)
            };

            _context.Professors.Add(professor);
            await _context.SaveChangesAsync();
            return professor;
        }

        // Authenticate professor by ID and password
        public async Task<Professor?> AuthenticateProfessorAsync(string professorId, string password)
        {
            var professor = await _context.Professors.FirstOrDefaultAsync(p => p.UtdId == professorId);

            if (professor != null && VerifyPassword(password, professor.PasswordHash))
            {
                return professor;
            }

            return null;
        }

        private static string CreatePasswordHash(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private static bool VerifyPassword(string password, string storedHash)
        {
            var computedHash = CreatePasswordHash(password);
            return computedHash.Equals(storedHash, StringComparison.OrdinalIgnoreCase);
        }

        // Update a professor by UtdId
        public async Task<Professor?> UpdateProfessorAsync(string utdId, ProfessorDto updatedProfessor)
        {
            var professor = await _context.Professors.FindAsync(utdId);
            if (professor == null)
            {
                return null;
            }

            professor.FirstName = updatedProfessor.FirstName ?? professor.FirstName;
            professor.LastName = updatedProfessor.LastName ?? professor.LastName;
            professor.Department = updatedProfessor.Department ?? professor.Department;
            professor.Email = updatedProfessor.Email ?? professor.Email;
            professor.PasswordHash = CreatePasswordHash(updatedProfessor.Password) ?? professor.PasswordHash;

            await _context.SaveChangesAsync();
            return professor;
        }

        // Delete a professor by UtdId
        public async Task<bool> DeleteProfessorAsync(string utdId)
        {
            var professor = await _context.Professors.FindAsync(utdId);
            if (professor == null)
            {
                return false;
            }

            _context.Professors.Remove(professor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}