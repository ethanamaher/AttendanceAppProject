using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.ApiService.Models;
using DbModels = AttendanceAppProject.ApiService.Data.Models;

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
            // Get the DB model professor
            var dbProfessor = await _context.Professors
                .FirstOrDefaultAsync(p => p.UtdId == professorId);

            if (dbProfessor != null)
            {
                // Convert to API model professor
                return new Professor
                {
                    Id = 0, // This is a placeholder since we don't use int IDs
                    ProfessorId = dbProfessor.UtdId,
                    FullName = $"{dbProfessor.FirstName} {dbProfessor.LastName}",
                    // These are placeholder values since they don't exist in the DB schema
                    PasswordHash = "placeholder",
                    Department = "Computer Science",
                    Email = $"{dbProfessor.FirstName.ToLower()}.{dbProfessor.LastName.ToLower()}@utdallas.edu"
                };
            }

            return null;
        }

        public async Task<Professor> GetProfessorByIdAsync(string professorId)
        {
            // Get the DB model professor
            var dbProfessor = await _context.Professors
                .FirstOrDefaultAsync(p => p.UtdId == professorId);

            if (dbProfessor != null)
            {
                // Convert to API model professor
                return new Professor
                {
                    Id = 0, // This is a placeholder since we don't use int IDs
                    ProfessorId = dbProfessor.UtdId,
                    FullName = $"{dbProfessor.FirstName} {dbProfessor.LastName}",
                    // These are placeholder values since they don't exist in the DB schema
                    PasswordHash = "placeholder",
                    Department = "Computer Science",
                    Email = $"{dbProfessor.FirstName.ToLower()}.{dbProfessor.LastName.ToLower()}@utdallas.edu"
                };
            }

            return null;
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