using EmployeeManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Application.Validators
{
    public class UpdateEmployeeDtoValidator : EmployeeValidatorBase<UpdateEmployeeDto>
    {
        public UpdateEmployeeDtoValidator()
        {
            ValidateFirstName(RuleFor(x => x.FirstName));
            ValidateLastName(RuleFor(x => x.LastName));
            ValidateEmail(RuleFor(x => x.Email));
            ValidateDepartment(RuleFor(x => x.Department));
            ValidateEmploymentStatus(RuleFor(x => x.EmploymentStatus));
        }
    }
}
