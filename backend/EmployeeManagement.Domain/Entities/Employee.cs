using EmployeeManagement.Domain.Enums;

namespace EmployeeManagement.Domain.Entities
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public EmploymentStatus EmploymentStatus { get; set; }
    }
}
