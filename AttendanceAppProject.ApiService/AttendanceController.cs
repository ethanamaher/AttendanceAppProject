using AttendanceAppProject.ApiService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceAppProject.Web.Controllers
{
    [ApiController]
    [Route("api/attendance")]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet]
        public async Task<ActionResult<List<AttendanceRecord>>> GetAttendanceRecords()
        {
            try
            {
                var records = await _attendanceService.GetAttendanceRecordsAsync();
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> SubmitAttendance([FromBody] AttendanceRecord record)
        {
            try
            {
                if (record == null)
                {
                    return BadRequest("Attendance record data is required");
                }

                var result = await _attendanceService.SubmitAttendanceAsync(record);

                if (result)
                {
                    return Ok();
                }
                else
                {
                    return StatusCode(500, "Failed to submit attendance record");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}