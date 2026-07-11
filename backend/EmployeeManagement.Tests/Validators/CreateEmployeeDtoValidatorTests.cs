using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Validators;
using FluentAssertions;
using Xunit;

namespace EmployeeManagement.Tests.Validators;

public class CreateEmployeeDtoValidatorTests
{
    private readonly CreateEmployeeDtoValidator _sut = new();

    [Fact]
    public void Validate_WithValidDto_HasNoErrors()
    {
        var dto = new CreateEmployeeDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "juan@testmail.com",
            Department = "IT",
            EmploymentStatus = "Active"
        };

        var result = _sut.Validate(dto);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("", "First Name")]
    public void Validate_WithMissingFirstName_HasError(string firstName, string _)
    {
        var dto = new CreateEmployeeDto
        {
            FirstName = firstName,
            LastName = "Doe",
            Email = "john@testmail.com",
            Department = "IT",
            EmploymentStatus = "Active"
        };

        var result = _sut.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateEmployeeDto.FirstName));
    }

    [Fact]
    public void Validate_WithInvalidEmail_HasError()
    {
        var dto = new CreateEmployeeDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "not-an-email",
            Department = "IT",
            EmploymentStatus = "Active"
        };

        var result = _sut.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateEmployeeDto.Email));
    }
   
}
