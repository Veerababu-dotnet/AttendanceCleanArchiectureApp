
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> _employeeRepo;

        public EmployeeService(IRepository<Employee> employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            if (string.IsNullOrWhiteSpace(employee.Name))
                  throw new Exception("Employee name is required");

            if (string.IsNullOrWhiteSpace(employee.Email))
                throw new Exception("Email is required");

            if (employee.DepartmentId <= 0)
                 throw new Exception("Invalid department");

                await _employeeRepo.AddAsync(employee);
            await _employeeRepo.SaveAsync(); 
        }

        public async Task<bool> EmployeeExistsAsync(int empId)
        {
            var employee = await _employeeRepo.GetByIdAsync(empId);
            return employee != null;
        }
    }
}
