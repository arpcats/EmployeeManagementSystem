using EmployeeManagement.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace EmployeeManagement.Application.Validators
{

    /// <summary>
    /// Note:
    /// This abstract base validator that contains common validation rules for employee related DTOs.
    /// shared by both CreateEmployeeDtoValidator and UpdateEmployeeDtoValidator. 
    /// It only provides reusable validation methods for derived classes.
    /// </summary>
    public abstract class EmployeeValidatorBase<T> : AbstractValidator<T>
    {
        protected void ValidateFirstName(IRuleBuilderInitial<T, string> rule)
        {
            rule.Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");
        }

        protected void ValidateLastName(IRuleBuilderInitial<T, string> rule)
        {
            rule.Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");
        }

        protected void ValidateEmail(IRuleBuilderInitial<T, string> rule)
        {
            rule.Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");
        }

        protected void ValidateDepartment(IRuleBuilderInitial<T, string> rule)
        {
            rule.Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Department is required.")
                .MaximumLength(50).WithMessage("Department cannot exceed 50 characters.");
        }

        protected void ValidateEmploymentStatus(IRuleBuilderInitial<T, string> rule)
        {
            rule.Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Employment status is required.")
                .Must(status => Enum.TryParse(typeof(Domain.Enums.EmploymentStatus), status, true, out _))
                .WithMessage("Invalid employment status.");
        }
    }
}
