/* Password API Controller
 * Handles HTTP GET and POST requests for attendance instances, allowing for retrieval and creation of passwords, as well as verifying if a password sent in by the client side exists in the database.
 * Written by Maaz Raza 
 */
using Microsoft.AspNetCore.Mvc;
using AttendanceAppProject.ApiService.Data.Models;
using Microsoft.EntityFrameworkCore;
using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.ApiService.Dto.Models;  // Updated namespace

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

        // Rest of the code remains the same...
    }
}