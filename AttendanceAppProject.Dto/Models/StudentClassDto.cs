using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceAppProject.Dto.Models
{
    public class StudentClassDto
    {
        // Nullable so that the client doesn't need to provide an ID when creating a new StudentClass mapping.
        // The server (API controller) will auto-generate a new GUID and assign it when saving to the database.
        // When retrieving data (from GET requests), this field will be populated with the actual value from the DB.
        public Guid? StudentClassId { get; set; }  // PK

        public string StudentId { get; set; } = null!; // FK1

        public Guid ClassId { get; set; } // FK2          

    }
}
