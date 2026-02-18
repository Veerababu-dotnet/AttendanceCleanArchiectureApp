using Application.DTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IAttendanceService
    {
        Task MarkAttendanceAsync(int empId, int statusId);
        Task<List<Attendance>> GetAttendanceAsync(int empId);
        Task DeleteAttendanceAsync(int empId, DateTime date);
        Task<MonthlyAttendanceSummary> GetMonthlyAttendanceSummaryAsync(int empId, int month, int year);
        Task<List<Attendance>> SearchAttendanceAsync(int empId, DateTime from, DateTime to);
    }
}
