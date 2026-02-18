using Application.Interfaces;
using Domain.Entities;
using Presentation.Helpers;

namespace Presentation.Menus
{
    public class MainMenu
    {
        private readonly IAttendanceService _attendanceService;
        private readonly IEmployeeService _employeeService;

        public MainMenu(
            IAttendanceService attendanceService,
            IEmployeeService employeeService)
        {
            _attendanceService = attendanceService;
            _employeeService = employeeService;
        }

        public async Task ShowAsync()
        {
            while (true)
            {
                Console.WriteLine("\n===== MENU =====");
                Console.WriteLine("1. Add Employee");
                Console.WriteLine("2. Mark Attendance");
                Console.WriteLine("3. View Attendance");
                Console.WriteLine("4. Delete Attendance");
                Console.WriteLine("5. View Monthly Attendance Summary");
                Console.WriteLine("6. Search Attendance by Date Range");
                Console.WriteLine("7. Exit");
                Console.Write("Select option: ");

                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid input");
                    continue;
                }

                try
                {
                    switch (choice)
                    {
                        case 1:
                            await AddEmployeeAsync();
                            break;

                        case 2:
                            await MarkAttendanceAsync();
                            break;

                        case 3:
                            await ViewAttendanceAsync();
                            break;

                        case 4:
                            await DeleteAttendanceAsync();
                            break;

                        case 5:
                            await ViewMonthlySummaryAsync();
                            break;

                        case 6:
                            await SearchAttendanceAsync();
                            break;

                        case 7:
                            Console.WriteLine("Exiting application...");
                            return;

                        default:
                            Console.WriteLine("Invalid option");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        

        private async Task AddEmployeeAsync()
        {
            var employee = new Employee
            {
                Name = ConsoleInput.ReadRequiredString("Name"),
                Email = ConsoleInput.ReadRequiredString("Email"),
                Phone = ConsoleInput.ReadRequiredString("Phone"),
                DepartmentId = ConsoleInput.ReadRequiredInt("Department Id")
            };

            await _employeeService.AddEmployeeAsync(employee);
            Console.WriteLine("Employee added successfully");
        }

        private async Task MarkAttendanceAsync()
        {
            var empId = ConsoleInput.ReadRequiredInt("Employee Id");
            var statusId = ConsoleInput.ReadRequiredInt("Status (1-Present, 2-Absent, 3-Late)");

            await _attendanceService.MarkAttendanceAsync(empId, statusId);
            Console.WriteLine("Attendance marked successfully");
        }

        private async Task ViewAttendanceAsync()
        {
            var empId = ConsoleInput.ReadRequiredInt("Employee Id");
            var records = await _attendanceService.GetAttendanceAsync(empId);

            if (!records.Any())
            {
                Console.WriteLine("No attendance records found");
                return;
            }

            Console.WriteLine("\nDate\t\tStatus");
            Console.WriteLine("------------------------");

            foreach (var r in records)
            {
                Console.WriteLine($"{r.AttendanceDate:yyyy-MM-dd}\t{r.Status.Code}");
            }
        }

        private async Task DeleteAttendanceAsync()
        {
            var empId = ConsoleInput.ReadRequiredInt("Employee Id");
            var date = ConsoleInput.ReadRequiredDate("Date");

            await _attendanceService.DeleteAttendanceAsync(empId, date);
            Console.WriteLine("Attendance deleted successfully");
        }

        private async Task ViewMonthlySummaryAsync()
        {
            var empId = ConsoleInput.ReadRequiredInt("Employee Id");
            var month = ConsoleInput.ReadRequiredInt("Month (1-12)");
            var year = ConsoleInput.ReadRequiredInt("Year");

            var summary = await _attendanceService
                .GetMonthlyAttendanceSummaryAsync(empId, month, year);

            Console.WriteLine("\nAttendance Summary");
            Console.WriteLine($"Present : {summary.PresentCount}");
            Console.WriteLine($"Absent  : {summary.AbsentCount}");
            Console.WriteLine($"Late    : {summary.LateCount}");
        }

        private async Task SearchAttendanceAsync()
        {
            var empId = ConsoleInput.ReadRequiredInt("Employee Id");
            var from = ConsoleInput.ReadRequiredDate("From Date");
            var to = ConsoleInput.ReadRequiredDate("To Date");

            var records = await _attendanceService
                .SearchAttendanceAsync(empId, from, to);

            if (!records.Any())
            {
                Console.WriteLine("No records found");
                return;
            }

            Console.WriteLine("\nDate\t\tStatus");
            Console.WriteLine("------------------------");

            foreach (var r in records)
            {
                var statuscode = r.Status?.Code ?? "UnKnown";
                Console.WriteLine($"{r.AttendanceDate:yyyy-MM-dd}\t{r.Status.Code}");
            }
        }
    }
}