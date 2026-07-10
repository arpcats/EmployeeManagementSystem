using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Application.Services;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Enums;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Tests.Services
{
    public class EmployeeServicesTests
    {
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
        private readonly IEmployeeService _employeeService;

        public EmployeeServicesTests()
        {
            ///Note: _employeeService is SUT system under test
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _employeeService = new EmployeeService(_employeeRepositoryMock.Object); //_SUT
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmployeeDtos()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee
                {
                    EmployeeId = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john@testmail.com",
                    Department = "IT",
                    EmploymentStatus = EmploymentStatus.Active
                }
            };

            _employeeRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(employees);

            // Act
            var result = await _employeeService.GetAllAsync(null);

            // Assert
            result.Should().HaveCount(1);
            result.First().FirstName.Should().Be("John");
            result.First().Email.Should().Be("john@test.com");
        }

        [Fact]
        public async Task CreateAsync_WithUniqueEmail_AddEmployeeAndReturnEmployeeDto()
        {
            // Arrange
            var dto = new CreateEmployeeDto
            {
                FirstName = "Jose",
                LastName = "Rizal",
                Email = "jose@testmail.com",
                Department = "Finance",
                EmploymentStatus = "Active"
            };
            _employeeRepositoryMock.Setup(r => r.EmailExistsAsync(dto.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            _employeeRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Employee e, CancellationToken _) => e);

            // Act
            var result = await _employeeService.CreateAsync(dto);

            // Assert
            result.FirstName.Should().Be("Jose");
            result.EmploymentStatus.Should().Be("Active");
            _employeeRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()), Times.Once);

        }
    }
}
