using Employee.API.Controllers;
using Employee.Application.Commands.Employee;
using Employee.Application.Queries.Employee;
using Employee.Core.Entities;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

public class EmployeesControllerTests
{
    private readonly Mock<ISender> _senderMock;
    private readonly EmployeesController _controller;

    public EmployeesControllerTests()
    {
        _senderMock = new Mock<ISender>();
        _controller = new EmployeesController(_senderMock.Object);
    }

    [Fact]
    public async Task GetEmployees_ShouldReturnOkResult()
    {
        // Arrange
        var employees = new List<EmployeeEntity>
        {
            new EmployeeEntity { EmployeeId = Guid.NewGuid(), Name = "John Doe" }
        };
        _senderMock.Setup(s => s.Send(It.IsAny<GetEmployeeQuery>(), default))
                   .ReturnsAsync(employees);

        // Act
        var result = await _controller.GetEmployees();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        okResult.Value.Should().BeEquivalentTo(employees);
    }

    [Fact]
    public async Task GetEmployeeById_ShouldReturnOkResult_WhenEmployeeExists()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var employee = new EmployeeEntity { EmployeeId = employeeId, Name = "John Doe" };
        _senderMock.Setup(s => s.Send(It.Is<GetEmployeeByIdQuery>(q => q.Id == employeeId), default))
                   .ReturnsAsync(employee);

        // Act
        var result = await _controller.GetEmployeeById(employeeId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        okResult.Value.Should().BeEquivalentTo(employee);
    }

    [Fact]
    public async Task AddEmployee_ShouldReturnOkResult()
    {
        // Arrange
        var employee = new EmployeeEntity { EmployeeId = Guid.NewGuid(), Name = "Jane Doe" };
        _senderMock.Setup(s => s.Send(It.IsAny<AddEmployeeCommand>(), default))
                   .ReturnsAsync(employee);

        // Act
        var result = await _controller.AddEmployeeAsync(employee);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        okResult.Value.Should().BeEquivalentTo(employee);
    }

    [Fact]
    public async Task UpdateEmployee_ShouldReturnOkResult_WhenEmployeeExists()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var employee = new EmployeeEntity { EmployeeId = employeeId, Name = "John Updated" };
        _senderMock.Setup(s => s.Send(It.IsAny<UpdateEmployeeCommand>(), default))
                   .ReturnsAsync(employee);

        // Act
        var result = await _controller.UpdateEmployee(employeeId, employee);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        okResult.Value.Should().BeEquivalentTo(employee);
    }

    [Fact]
    public async Task UpdateEmployee_ShouldReturnBadRequest_WhenEmployeeNotFound()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var employee = new EmployeeEntity { EmployeeId = employeeId, Name = "Non-Existent" };
        _senderMock.Setup(s => s.Send(It.IsAny<UpdateEmployeeCommand>(), default))
                   .ReturnsAsync((EmployeeEntity)null);

        // Act
        var result = await _controller.UpdateEmployee(employeeId, employee);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        badRequestResult.Value.Should().Be("Not found the entity to update");
    }

    [Fact]
    public async Task DeleteEmployee_ShouldReturnOkResult()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        _senderMock.Setup(s => s.Send(It.IsAny<DeleteEmployeeCommand>(), default))
                   .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteEmployee(employeeId);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
}
