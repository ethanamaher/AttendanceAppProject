using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceAppProject.ApiService
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorAuthController : ControllerBase
    {
        private readonly IProfessorAuthService _authService;

        public ProfessorAuthController(IProfessorAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.ProfessorId) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest("Username and password are required");
                }

                var professor = await _authService.AuthenticateProfessorAsync(request.ProfessorId, request.Password);

                if (professor == null)
                {
                    return Unauthorized("Invalid credentials");
                }

                // Update last login date
                await _authService.UpdateLastLoginAsync(professor.ProfessorId);

                return Ok(new
                {
                    professor.ProfessorId,
                    professor.FullName,
                    professor.Department,
                    professor.Email,
                    LastLogin = professor.LastLoginDate
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        public class LoginRequest
        {
            public string ProfessorId { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
    }
}