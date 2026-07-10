using EmployeeManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Application.Validators
{
    public class CreateEmployeeDtoValidator : EmployeeValidatorBase<CreateEmployeeDto>
    {
        public CreateEmployeeDtoValidator()
        { 
            ValidateFirstName(RuleFor(x => x.FirstName));
            ValidateLastName(RuleFor(x => x.LastName));
            ValidateEmail(RuleFor(x => x.Email));
            ValidateDepartment(RuleFor(x => x.Department));
            ValidateEmploymentStatus(RuleFor(x => x.EmploymentStatus));
        }
    }
}
