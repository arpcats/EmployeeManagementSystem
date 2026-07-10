using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Infrastructure
{
    public class EmployeeRepository
    {
        private readonly AppDbContext _appDbContext;

        public EmployeeRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Employee>> GetAllAsync(CancellationToken cancellation = default)
        {
            return await _appDbContext.Employees
                .AsNoTracking()
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .ToListAsync(cancellation);
        }

        public async Task<Employee?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _appDbContext.Employees
                .FirstOrDefaultAsync(e => e.EmployeeId == id, cancellationToken);
        }

        public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        {
            var query = _appDbContext.Employees.Where(e => e.Email.ToLower() == email.ToLower());

            return await query.AnyAsync(cancellationToken);
        }

        public async Task<Employee> AddAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            _appDbContext.Employees.Add(employee);
            await _appDbContext.SaveChangesAsync(cancellationToken);
            return employee;
        }

        public async Task<Employee> UpdateAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            _appDbContext.Employees.Update(employee);
            await _appDbContext.SaveChangesAsync(cancellationToken);
            return employee;
        }

    }
}
