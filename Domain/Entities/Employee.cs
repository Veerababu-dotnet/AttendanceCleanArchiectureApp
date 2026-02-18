using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;
    }
}
