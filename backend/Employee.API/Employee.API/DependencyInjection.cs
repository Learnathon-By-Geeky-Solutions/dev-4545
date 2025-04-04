﻿using Employee.Application;
using Employee.Infrastructure;

namespace Employee.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppDI(this IServiceCollection services,IConfiguration configuration) {
            services.AddApplicationDI()
                .AddInfrastructureDI(configuration);

            return services;
        }
    }
}
