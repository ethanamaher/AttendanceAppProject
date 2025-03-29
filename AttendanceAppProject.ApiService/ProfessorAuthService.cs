using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
            if (string.IsNullOrEmpty(professorId) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var professor = await _context.Professors
                .FirstOrDefaultAsync(p => p.ProfessorId == professorId);

            if (professor == null)
            {
                return null;
            }

            // Verify password hash
            if (VerifyPasswordHash(password, professor.PasswordHash))
            {
                return professor;
            }

            return null;
        }

        public async Task<bool> UpdateLastLoginAsync(string professorId)
        {
            var professor = await _context.Professors
                .FirstOrDefaultAsync(p => p.ProfessorId == professorId);

            if (professor == null)
            {
                return false;
            }

            professor.LastLoginDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            // For simplicity, we're using a basic SHA256 hash
            // In a production app, use a proper password hashing library with salt
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return storedHash == builder.ToString();
            }
        }

        // Helper method to create password hash (for seeding data)
        public static string CreatePasswordHash(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}