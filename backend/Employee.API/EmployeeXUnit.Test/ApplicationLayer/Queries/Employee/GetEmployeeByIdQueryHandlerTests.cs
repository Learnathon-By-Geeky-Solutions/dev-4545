using System;
using System.Threading;
using System.Threading.Tasks;
using Employee.Application.Queries.Employee;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using Moq;
using Xunit;

namespace Employee.Application.Tests.Queries.Employee
{
    public class GetEmployeeByIdQueryHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Return_EmployeeEntity_When_Found()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var employee = new EmployeeEntity
            {
                EmployeeId = employeeId,
                Name = "John Doe",
                Stack = "DotNet",
                Email = "john.doe@example.com",
                Salt = "random-salt",
                Password = "hashed-password",
                DateOfJoin = DateTime.UtcNow,
                Phone = "0123456789"
            };

            var repositoryMock = new Mock<IEmployeeRepository>();
            repositoryMock
                .Setup(repo => repo.GetEmployeeById(employeeId))
                .ReturnsAsync(employee);

            var handler = new GetEmployeeByIdQueryHandler(repositoryMock.Object);
            var query = new GetEmployeeByIdQuery(employeeId);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(employee.EmployeeId, result.EmployeeId);
            Assert.Equal(employee.Name, result.Name);
            Assert.Equal(employee.Email, result.Email);
        }

        [Fact]
        public async Task Handle_Should_Propagate_Exception_When_RepositoryThrows()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var repositoryMock = new Mock<IEmployeeRepository>();
            repositoryMock
                .Setup(repo => repo.GetEmployeeById(employeeId))
                .ThrowsAsync(new Exception("Employee not found"));

            var handler = new GetEmployeeByIdQueryHandler(repositoryMock.Object);
            var query = new GetEmployeeByIdQuery(employeeId);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                handler.Handle(query, CancellationToken.None));
            Assert.Equal("Employee not found", exception.Message);
        }
    }
}
