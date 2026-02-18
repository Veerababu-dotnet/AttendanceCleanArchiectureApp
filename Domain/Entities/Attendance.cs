using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Attendance
    {
        
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
        public DateTime AttendanceDate { get; set; }
        public int StatusId { get; set; }
        public AttendanceStatus Status { get; set; } = null!;
    }
}
