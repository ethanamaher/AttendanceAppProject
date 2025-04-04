using Microsoft.AspNetCore.Mvc;
using AttendanceAppProject.ApiService.Data.Models;
using Microsoft.EntityFrameworkCore;
using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.Dto.Models;

// API Controller for Password

namespace AttendanceAppProject.ApiService.Controllers
{
    [Route("api/[controller]")] // Automatically becomes "api/password"
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PasswordController(ApplicationDbContext context)
        {
            _context = context;
        }

        /* GET: api/password
         * Get all passwords
         * - request body: none
         * - response body: Passwords
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Password>>> GetPasswords()
        {
            return await _context.Passwords.ToListAsync();
        }

        /* POST: api/Password
         * Add a password to the database
         * - request body: PasswordDto
         * - response body: Password
         */
        [HttpPost]
        public async Task<ActionResult<Password>> AddPassword([FromBody] PasswordDto dto)
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
            return CreatedAtAction(nameof(GetPasswords), new { id = password.PasswordId }, password);
        }

        /* POST: api/Password/validate 
         * Validates a password based on ClassId, Password text, and the date assigned. Client side sends this data over in a PasswordDto object
         * - request body: PasswordDto (containing fields ClassId, PasswordText, Date based upon student input form)
         * - resposne body: true if valid, false if not
         */

        [HttpPost("validate")]
        public async Task<ActionResult<bool>> ValidatePassword([FromBody] PasswordDto dto)
        {
            var exists = await _context.Passwords.AnyAsync(p =>
                p.ClassId == dto.ClassId &&
                p.PasswordText.ToLower() == dto.PasswordText.ToLower() &&
                p.DateAssigned == dto.DateAssigned
            );

            return Ok(exists); // true if valid, false if not
        }

    }
}

