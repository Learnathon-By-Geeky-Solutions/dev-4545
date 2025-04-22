using Employee.API.Controllers;
using Employee.Application.Commands.Salary;
using Employee.Application.Queries.Salary;
using Employee.Core.Entities;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeXUnit.Test.PresentationLayer.Controllers
{
    public class SalaryControllerTests
    {
        private readonly Mock<ISender> _senderMock;
        private readonly SalaryController _controller;

        public SalaryControllerTests()
        {
            _senderMock = new Mock<ISender>();
            _controller = new SalaryController(_senderMock.Object);
        }

        [Fact]
        public async Task InsertSalary_ShouldReturnOkResult()
        {
            // Arrange
            var salary = new SalaryEntity
            {
                SalaryId = Guid.NewGuid(),
                Amount = 5000.0f,
                SalaryDate = DateOnly.FromDateTime(DateTime.Now),
                EmployeeId = Guid.NewGuid()
            };
            _senderMock.Setup(s => s.Send(It.IsAny<AddSalaryCommand>(), default))
                       .ReturnsAsync(salary);

            // Act
            var result = await _controller.InsertSalary(salary);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeEquivalentTo(salary);
        }

        [Fact]
        public async Task GetSalaries_ShouldReturnOkResult()
        {
            // Arrange
            var salaries = new List<SalaryEntity>
            {
                new SalaryEntity
                {
                    SalaryId = Guid.NewGuid(),
                    Amount = 5000.0f,
                    SalaryDate = DateOnly.FromDateTime(DateTime.Now),
                    EmployeeId = Guid.NewGuid()
                }
            };
            _senderMock.Setup(s => s.Send(It.IsAny<GetSalaryQuery>(), default))
                       .ReturnsAsync(salaries);

            // Act
            var result = await _controller.GetSalaries();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeEquivalentTo(salaries);
        }

        [Fact]
        public async Task GetSalaryByEmpId_ShouldReturnOkResult_WhenSalaryExists()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var salary = new SalaryEntity
            {
                SalaryId = Guid.NewGuid(),
                Amount = 5000.0f,
                SalaryDate = DateOnly.FromDateTime(DateTime.Now),
                EmployeeId = employeeId
            };
            _senderMock.Setup(s => s.Send(It.Is<GetSalariesByEmployeeIdQuery>(q => q.EmployeeId == employeeId), default))
                       .ReturnsAsync(salary);

            // Act
            var result = await _controller.GetSalaryByEmpId(employeeId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeEquivalentTo(salary);
        }

        [Fact]
        public async Task UpdateSalaryByEmpId_ShouldReturnOkResult_WhenSalaryExists()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var salary = new SalaryEntity
            {
                SalaryId = Guid.NewGuid(),
                Amount = 5500.0f,
                SalaryDate = DateOnly.FromDateTime(DateTime.Now),
                EmployeeId = employeeId
            };
            _senderMock.Setup(s => s.Send(It.IsAny<UpdateSalaryCommand>(), default))
                       .ReturnsAsync(salary);

            // Act
            var result = await _controller.UpdateSalaryByEmpId(employeeId, salary);


            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeEquivalentTo(salary);
        }

        [Fact]
        public async Task UpdateSalaryByEmpId_ShouldReturnBadRequest_WhenSalaryNotFound()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var salary = new SalaryEntity
            {
                SalaryId = Guid.NewGuid(),
                Amount = 6000.0f,
                SalaryDate = DateOnly.FromDateTime(DateTime.Now),
                EmployeeId = employeeId
            };
            _senderMock.Setup(s => s.Send(It.IsAny<UpdateSalaryCommand>(), default))
                   .ReturnsAsync((SalaryEntity?)null!);


            // Act
            var result = await _controller.UpdateSalaryByEmpId(employeeId, salary);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            badRequestResult.Value.Should().Be("Entity not found to update.");
        }

        [Fact]
        public async Task DeleteSalaryByEmpId_ShouldReturnOkResult()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            _senderMock.Setup(s => s.Send(It.IsAny<DeleteSalaryByEmpIdCommand>(), default))
                       .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteSalaryByEmpId(employeeId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
