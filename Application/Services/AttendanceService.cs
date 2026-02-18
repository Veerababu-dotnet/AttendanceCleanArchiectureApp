
using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IRepository<Attendance> _attendanceRepo;
        private readonly IRepository<Employee> _employeeRepo;

        public AttendanceService(IRepository<Attendance> attendanceRepo,
            IRepository<Employee> employeeRepo) 
        {
            _attendanceRepo = attendanceRepo;
            _employeeRepo = employeeRepo;
        }

        public async Task MarkAttendanceAsync(int empId, int statusId)
        {
            if (statusId < 1 || statusId > 3)
               throw new Exception("Invalid attendance status");



            var employeeExists = await _employeeRepo.Query()
              .AnyAsync(e => e.Id == empId);


                if (!employeeExists)
                   throw new Exception("Invalid employee id. enter correct one");

                var attendance = new Attendance
                {
                EmployeeId = empId,
                StatusId = statusId,
                AttendanceDate = DateTime.Now
                };

            await _attendanceRepo.AddAsync(attendance);
            await _attendanceRepo.SaveAsync();
        }

        public async Task<List<Attendance>> GetAttendanceAsync(int empId)
        {
            return await _attendanceRepo.Query()
                .Include(a => a.Status)
                .Where(a => a.EmployeeId == empId)
                .ToListAsync();
        }

        public async Task DeleteAttendanceAsync(int empId, DateTime date)
        {
            var record = await _attendanceRepo.Query()
                .FirstOrDefaultAsync(a => a.EmployeeId == empId && a.AttendanceDate.Date == date.Date);

            if (record == null)
                 throw new Exception("Attendance record not found");

           
                await _attendanceRepo.DeleteAsync(record);
                await _attendanceRepo.SaveAsync();
            
        }

        public async Task<MonthlyAttendanceSummary> GetMonthlyAttendanceSummaryAsync(int empId, int month, int year)
        {
            var records = await _attendanceRepo.Query()
                .Include(a => a.Status)
                .Where(a => a.EmployeeId == empId && a.AttendanceDate.Month == month && a.AttendanceDate.Year == year)
                .ToListAsync();

            return new MonthlyAttendanceSummary
            {

                PresentCount = records.Count(r => r.StatusId == 1),
                AbsentCount = records.Count(r => r.StatusId == 2),
                LateCount = records.Count(r => r.StatusId == 3)
            };
        }

        public async Task<List<Attendance>> SearchAttendanceAsync(int empId, DateTime from, DateTime to)
        {
            return await _attendanceRepo.Query()
                .Include(a =>a.Status)
                .Where(a => a.EmployeeId == empId && a.AttendanceDate >= from && a.AttendanceDate <= to)
                .ToListAsync();
        }
    }
}
