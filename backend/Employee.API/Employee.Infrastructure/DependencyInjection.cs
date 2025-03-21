﻿using Employee.Core.Interfaces;
using Employee.Infrastructure.Data;
using Employee.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Employee.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            return services;
        }
    }
}
