﻿/* Password API Controller
 * Handles HTTP GET, POST, PUT, and DELETE requests for passwords, allowing for retrieval and creation of passwords, as well as verifying if a password sent in by the client side exists in the database.
 * Written by Maaz Raza 
 */

using Microsoft.AspNetCore.Mvc;
using AttendanceAppProject.ApiService.Data.Models;
using Microsoft.EntityFrameworkCore;
using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.ApiService.Services;
using AttendanceAppProject.Dto.Models;

// API Controller for Password

namespace AttendanceAppProject.ApiService.Controllers
{
    [Route("api/[controller]")] // Automatically becomes "api/password"
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private readonly PasswordService _service;

        // Dependency Injection of the PasswordService
        public PasswordController(PasswordService service)
        {
            _service = service;
        }

        /* GET: api/password
         * Get all passwords
         * - request body: none
         * - response body: <IEnumerable<Password>>
         */
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Password>>> GetPasswords()
        {
            return Ok(await _service.GetPasswordsAsync());
        }

        /* POST: api/Password
         * Add a password to the database
         * - request body: PasswordDto
         * - response body: Password
         */
        [HttpPost]
        public async Task<ActionResult<Password>> AddPassword([FromBody] PasswordDto dto)
        {
            var password = await _service.AddPasswordAsync(dto);
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
            var exists = await _service.ValidatePasswordAsync(dto);

            return Ok(exists); // true if valid, false if not
        }

        /* PUT: api/Password/{classId}
         * Update a password by class ID
         * - request body: PasswordDto
         * - response body: Password
         */
        [HttpPut("{classId}")]
        public async Task<ActionResult<Password>> UpdatePassword(Guid classId, [FromBody] PasswordDto updatedPassword)
        {
            System.Diagnostics.Debug.WriteLine($"{updatedPassword.PasswordText}");
            System.Diagnostics.Debug.WriteLine($"Attempting to update the password for class: {classId}");
            var password = await _service.UpdatePasswordAsync(classId, updatedPassword);
            if (password == null)
                return NotFound();

            return Ok(password);
        }

        /* DELETE: api/Password/{id}
         * Delete a password by ID
         * - request body: none
         * - response body: HttpResponse
         */
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePassword(Guid id)
        {
            var success = await _service.DeletePasswordAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }

        /* GET: api/password/class/{classId}
         * Get a password for a given class provided its classId
         * - request body: Guid classId
         * - request response: PasswordDto
         */
        [HttpGet("class/{classId}")]
        public async Task<ActionResult<Password>> GetPasswordByClassId(Guid classId)
        {
            var success = await _service.GetPasswordByClassIdAsync(classId);

            // if notfound, no password exists
            if (success == null)
                return null;

            return Ok(success);
        }

    }
}

