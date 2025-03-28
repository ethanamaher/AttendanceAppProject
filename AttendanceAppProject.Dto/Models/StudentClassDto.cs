using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceAppProject.Dto.Models
{
    public class StudentClassDto
    {
        public Guid StudentClassId { get; set; } 

        public string StudentId { get; set; } = null!;  

        public Guid ClassId { get; set; }           

    }
}
