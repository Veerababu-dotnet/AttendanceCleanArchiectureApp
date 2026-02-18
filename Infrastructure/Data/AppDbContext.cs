using Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        private readonly DatabaseSettings _settings;


        public AppDbContext(DbContextOptions<AppDbContext> options)
          
            : base(options)
        {
          
        }

        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Attendance> Attendances => Set<Attendance>();
        public DbSet<AttendanceStatus> AttendanceStatus => Set<AttendanceStatus>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_settings.ConnectionString);
            }
        }
    }

}
