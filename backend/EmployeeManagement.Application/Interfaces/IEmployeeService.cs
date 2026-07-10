using EmployeeManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDto>> GetAllAsync(string? search, CancellationToken cancellationToken = default);
        Task<EmployeeDto> GetByIdAsync(int id, CancellationToken cancellationToken = default); 
        Task<EmployeeDto> CreateAsync(CreateEmployeeDto createEmployeeDto, CancellationToken cancellationToken = default);
        Task<EmployeeDto> UpdateAsync(int id, UpdateEmployeeDto updateEmployeeDto, CancellationToken cancellationToken = default);
    }
}
