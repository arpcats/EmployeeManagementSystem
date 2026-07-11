using EmployeeManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Infrastructure.Persistence.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");

            builder.HasKey(e => e.EmployeeId);

            builder.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(e => e.LastName).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Email).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Department).IsRequired().HasMaxLength(50);
            builder.Property(e => e.EmploymentStatus).IsRequired().HasConversion<string>().HasMaxLength(20);

            builder.HasIndex(e => e.Email).IsUnique();

            // Seed data so the API/UI has something to show immediately after
            // the first migration, without requiring manual data entry.
            builder.HasData(
                new Employee
                {
                    EmployeeId = 1,
                    FirstName = "John",
                    LastName = "Joe",
                    Email = "john.doe@testmail.com",
                    Department = "IT",
                    EmploymentStatus = Domain.Enums.EmploymentStatus.Active
                },
                new Employee
                {
                    EmployeeId = 2,
                    FirstName = "Tony",
                    LastName = "Payumo",
                    Email = "tony.payumo@testmail.com",
                    Department = "IT",
                    EmploymentStatus = Domain.Enums.EmploymentStatus.OnLeave
                },
                new Employee
                {
                    EmployeeId = 3,
                    FirstName = "Jose",
                    LastName = "Rizal",
                    Email = "jose.rizal@testmail.com",
                    Department = "Finance",
                    EmploymentStatus = Domain.Enums.EmploymentStatus.Active
                }
            );

        }
    }
}
