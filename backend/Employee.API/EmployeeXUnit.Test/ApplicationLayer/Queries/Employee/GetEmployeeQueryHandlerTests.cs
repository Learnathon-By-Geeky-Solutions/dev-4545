using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Employee.Application.Queries.Employee;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using Moq;
using Xunit;

namespace Employee.Application.Tests.Queries.Employee
{
    public class GetEmployeeQueryHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Return_List_Of_Employees()
        {
            // Arrange
            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity
                {
                    EmployeeId = Guid.NewGuid(),
                    Name = "Alice",
                    Stack = "Java",
                    Email = "alice@example.com",
                    Salt = "salt1",
                    Password = "password1",
                    DateOfJoin = DateTime.UtcNow,
                    Phone = "0111111111"
                },
                new EmployeeEntity
                {
                    EmployeeId = Guid.NewGuid(),
                    Name = "Bob",
                    Stack = "DotNet",
                    Email = "bob@example.com",
                    Salt = "salt2",
                    Password = "password2",
                    DateOfJoin = DateTime.UtcNow,
                    Phone = "0222222222"
                }
            };

            var repositoryMock = new Mock<IEmployeeRepository>();
            repositoryMock
                .Setup(repo => repo.GetEmployees())
                .ReturnsAsync(employees);

            var handler = new GetEmployeeQueryHandler(repositoryMock.Object);
            var query = new GetEmployeeQuery();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            var resultList = new List<EmployeeEntity>(result);
            Assert.Equal(2, resultList.Count);
            Assert.Contains(resultList, emp => emp.Name == "Alice");
            Assert.Contains(resultList, emp => emp.Name == "Bob");
        }

        [Fact]
        public async Task Handle_Should_Propagate_Exception_When_RepositoryThrows()
        {
            // Arrange
            var repositoryMock = new Mock<IEmployeeRepository>();
            repositoryMock
                .Setup(repo => repo.GetEmployees())
                .ThrowsAsync(new Exception("Error retrieving employees"));

            var handler = new GetEmployeeQueryHandler(repositoryMock.Object);
            var query = new GetEmployeeQuery();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                handler.Handle(query, CancellationToken.None));
            Assert.Equal("Error retrieving employees", exception.Message);
        }
    }
}
