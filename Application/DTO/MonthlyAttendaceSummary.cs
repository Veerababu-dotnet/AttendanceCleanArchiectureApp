using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTO
{
    public record MonthlyAttendanceSummary
    {
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public int LateCount { get; set; }
    
     public MonthlyAttendanceSummary() { }


        //optional
        public MonthlyAttendanceSummary(int presentCount, int absentCount, int lateCount)
        {
            PresentCount = presentCount; 
            AbsentCount = absentCount; 
            LateCount = lateCount;
        }

    }

}