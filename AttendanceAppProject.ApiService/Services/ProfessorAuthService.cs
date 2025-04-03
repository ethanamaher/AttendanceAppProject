using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.ApiService.Models;

namespace AttendanceAppProject.ApiService
{
    public class ProfessorAuthService : IProfessorAuthService
    {
        private readonly ApplicationDbContext _context;

        public ProfessorAuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Professor> AuthenticateProfessorAsync(string professorId, string password)
        {
            var professor = await _context.Professors
                .FirstOrDefaultAsync(p => p.ProfessorId == professorId);

            if (professor != null && VerifyPassword(password, professor.PasswordHash))
            {
                return professor;
            }

            return null;
        }

        public async Task<Professor> GetProfessorByIdAsync(string professorId)
        {
            return await _context.Professors
                .FirstOrDefaultAsync(p => p.ProfessorId == professorId);
        }

        public static string CreatePasswordHash(string password)
        {
            // For simplicity, we're using SHA256 hashing
            // In a production environment, use a more secure method like bcrypt
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            string computedHash = CreatePasswordHash(password);
            return computedHash.Equals(passwordHash, StringComparison.OrdinalIgnoreCase);
        }
    }
}