using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceAppProject.Dto.Models
{
    public class StudentClassDto
    {
        public Guid StudentClassId { get; set; }  // PK

        public string StudentId { get; set; } = null!; // FK1

        public Guid ClassId { get; set; } // FK2          

    }
}
