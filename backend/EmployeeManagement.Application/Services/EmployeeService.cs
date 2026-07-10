using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<List<EmployeeDto>> GetAllAsync(string? search, CancellationToken cancellationToken = default)
        {
            var employees = await _employeeRepository.GetAllAsync(cancellationToken);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();
                employees = employees.Where(e =>
                    e.FirstName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    e.LastName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    $"{e.FirstName} {e.FirstName}".Contains(term, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            return employees.Select(e => new EmployeeDto
            {
                EmployeeId = e.EmployeeId,
                FirstName = e.FirstName,
                LastName = e.FirstName,
                Email = e.Email,
                Department = e.Department,
                EmploymentStatus = e.EmploymentStatus.ToString(),
            }).ToList();
        }

        public async Task<EmployeeDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var employee = await _employeeRepository.GetByIdAsync(id, cancellationToken);
            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {id} not found.");
            }
            return new EmployeeDto
            {
                EmployeeId = employee.EmployeeId,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Department = employee.Department,
                EmploymentStatus = employee.EmploymentStatus.ToString(),
            };
        }

        public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto createEmployeeDto, CancellationToken cancellationToken = default)
        {
            if (await _employeeRepository.EmailExistsAsync(createEmployeeDto.Email, cancellationToken))
            {
                throw new InvalidOperationException($"An employee with email {createEmployeeDto.Email} already exists.");
            }
            var employee = new Employee
            {
                FirstName = createEmployeeDto.FirstName,
                LastName = createEmployeeDto.LastName,
                Email = createEmployeeDto.Email,
                Department = createEmployeeDto.Department,
                EmploymentStatus = Enum.Parse<EmploymentStatus>(createEmployeeDto.EmploymentStatus, true),
            };
            var createdEmployee = await _employeeRepository.AddAsync(employee, cancellationToken);
            return new EmployeeDto
            {
                EmployeeId = createdEmployee.EmployeeId,
                FirstName = createdEmployee.FirstName,
                LastName = createdEmployee.LastName,
                Email = createdEmployee.Email,
                Department = createdEmployee.Department,
                EmploymentStatus = createdEmployee.EmploymentStatus.ToString(),
            };
        }

        public async Task<EmployeeDto> UpdateAsync(int id, UpdateEmployeeDto updateEmployeeDto, CancellationToken cancellationToken = default)
        {
            var employee = await _employeeRepository.GetByIdAsync(id, cancellationToken);
            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with ID {id} not found.");
            }
            if (!string.Equals(employee.Email, updateEmployeeDto.Email, StringComparison.OrdinalIgnoreCase) &&
                await _employeeRepository.EmailExistsAsync(updateEmployeeDto.Email, cancellationToken))
            {
                throw new InvalidOperationException($"An employee with email {updateEmployeeDto.Email} already exists.");
            }
            employee.FirstName = updateEmployeeDto.FirstName;
            employee.LastName = updateEmployeeDto.LastName;
            employee.Email = updateEmployeeDto.Email;
            employee.Department = updateEmployeeDto.Department;
            employee.EmploymentStatus = Enum.Parse<EmploymentStatus>(updateEmployeeDto.EmploymentStatus, true);
            var updatedEmployee = await _employeeRepository.UpdateAsync(employee, cancellationToken);
            return new EmployeeDto
            {
                EmployeeId = updatedEmployee.EmployeeId,
                FirstName = updatedEmployee.FirstName,
                LastName = updatedEmployee.LastName,
                Email = updatedEmployee.Email,
                Department = updatedEmployee.Department,
                EmploymentStatus = updatedEmployee.EmploymentStatus.ToString(),
            };
        }
    }
}
