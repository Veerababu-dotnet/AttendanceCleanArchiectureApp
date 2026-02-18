using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IEmployeeService
    {
        Task AddEmployeeAsync(Employee employee);
        Task<bool> EmployeeExistsAsync(int empId);
    }
}
