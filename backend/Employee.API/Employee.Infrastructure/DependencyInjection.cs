using Employee.Application.Common.Authorization;
using Employee.Application.Common.Interfaces;
using Employee.Application.Interfaces;
using Employee.Core.Interfaces;
using Employee.Infrastructure.Data;
using Employee.Infrastructure.Repositories;
using Employee.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
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
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("RedisURL");
                options.InstanceName = "MyApp:";

            });
            services.AddAuthorizationCore(options =>
            {
                options.AddPolicy("CanModifyOwnEmployee", policy =>
                    policy.Requirements.Add(new OwnEmployeeRequirement()));
            });


            services.AddSingleton<IAuthorizationHandler, OwnEmployeeHandler>();

            // Register your handler (from Application via DI)


            services.AddScoped<IAccessTokenService,AccessTokenService>();
            services.AddScoped<ICacheService, RedisCacheService>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ITasksRepository, TaskRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<ISalaryRepository, SalaryRepository>();
            services.AddScoped<ILeaveRepository, LeaveRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IFeatureRepository, FeatureRepository>();
            services.AddScoped<IPerformanceRepository, PerformanceRepository>();
            return services;
        }
    }
}
