using System;
using System.Collections.Generic;
using Employee.Infrastructure;
using Employee.Infrastructure.Data;
using Employee.Infrastructure.Repositories;
using Employee.Infrastructure.Services;
using Employee.Application.Common.Interfaces;
using Employee.Application.Interfaces;
using Employee.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace EmployeeXUnit.Test.InfrastructureLayer
{
    public class DependencyInjectionTests
    {
        private readonly IServiceProvider _provider;

        public DependencyInjectionTests()
        {
            // arrange an in-memory IConfiguration
            var inMemorySettings = new Dictionary<string, string> {
                {"ConnectionStrings:DefaultConnection", "Server=(localdb)\\mssqllocaldb;Database=TestDb;Trusted_Connection=True;"},
                {"ConnectionStrings:RedisURL", "localhost:6379"}
            };
            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            // build the service provider
            var services = new ServiceCollection();

            // register IConfiguration itself so any constructors asking for it can be satisfied
            services.AddSingleton(config);

            // register your infrastructure DI
            services.AddInfrastructureDI(config);

            _provider = services.BuildServiceProvider();
        }

        public static IEnumerable<object[]> ServiceMappings => new[]
        {
            new object[] { typeof(IAccessTokenService),      typeof(AccessTokenService) },
            new object[] { typeof(ICacheService),            typeof(RedisCacheService) },
            new object[] { typeof(IEmployeeRepository),      typeof(EmployeeRepository) },
            new object[] { typeof(ITasksRepository),         typeof(TaskRepository) },
            new object[] { typeof(IRefreshTokenRepository),  typeof(RefreshTokenRepository) },
            new object[] { typeof(ISalaryRepository),        typeof(SalaryRepository) },
            new object[] { typeof(ILeaveRepository),         typeof(LeaveRepository) },
            new object[] { typeof(IProjectRepository),       typeof(ProjectRepository) },
            new object[] { typeof(IFeatureRepository),       typeof(FeatureRepository) },
            new object[] { typeof(IPerformanceRepository),   typeof(PerformanceRepository) },
        };

        [Theory]
        [MemberData(nameof(ServiceMappings))]
        public void All_Scoped_Services_Are_Registered(Type serviceType, Type implType)
        {
            // act
            var svc = _provider.GetService(serviceType);

            // assert
            Assert.NotNull(svc);
            Assert.IsType(implType, svc);
        }

        [Fact]
        public void AppDbContext_Is_Registered_And_Uses_SqlServer()
        {
            // act
            var dbContext = _provider.GetService<AppDbContext>();
            Assert.NotNull(dbContext);

            // EF Core helper to check provider
            Assert.True(dbContext.Database.IsSqlServer(),
                "AppDbContext should be configured to use SQL Server.");
        }

        [Fact]
        public void RedisCacheOptions_Are_Configured_Correctly()
        {
            var opts = _provider.GetRequiredService<IOptions<RedisCacheOptions>>().Value;
            Assert.Equal("localhost:6379", opts.Configuration);
            Assert.Equal("MyApp:", opts.InstanceName);
        }

        [Fact]
        public void Scoped_Lifetime_Works_As_Expected()
        {
            // create two scopes
            using (var scope1 = _provider.CreateScope())
            using (var scope2 = _provider.CreateScope())
            {
                var svc1A = scope1.ServiceProvider.GetService<IEmployeeRepository>();
                var svc1B = scope1.ServiceProvider.GetService<IEmployeeRepository>();
                var svc2 = scope2.ServiceProvider.GetService<IEmployeeRepository>();

                // same scope -> same instance
                Assert.Same(svc1A, svc1B);

                // different scopes -> different instances
                Assert.NotSame(svc1A, svc2);
            }
        }
    }
}
