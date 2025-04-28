using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Employee.Application.Commands.Salary;
using Employee.Application.Queries.Salary;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using MediatR;
using Moq;
using Xunit;

namespace EmployeeXUnit.Test.ApplicationLayer
{
    public class SalaryHandlersTests
    {
        private readonly Mock<ISalaryRepository> _repoMock;
        private readonly CancellationToken _ct = CancellationToken.None;

        public SalaryHandlersTests()
        {
            _repoMock = new Mock<ISalaryRepository>();
        }

        [Fact]
        public async Task AddSalaryCommandHandler_Should_Call_AddSalary_And_Return_Entity()
        {
            // Arrange
            var salary = new SalaryEntity
            {
                SalaryId = Guid.NewGuid(),
                Amount = 1000f,
                SalaryDate = DateOnly.FromDateTime(DateTime.Today),
                EmployeeId = Guid.NewGuid()
            };
            _repoMock
                .Setup(r => r.AddSalary(salary))
                .ReturnsAsync(salary);

            var handler = new AddSalaryCommandHandler(_repoMock.Object);
            var command = new AddSalaryCommand(salary);

            // Act
            var result = await handler.Handle(command, _ct);

            // Assert
            Assert.Equal(salary, result);
            _repoMock.Verify(r => r.AddSalary(salary), Times.Once);
        }

        [Fact]
        public async Task UpdateSalaryCommandHandler_Should_Call_UpdateSalary_And_Return_Entity()
        {
            // Arrange
            var empId = Guid.NewGuid();
            var updated = new SalaryEntity
            {
                SalaryId = Guid.NewGuid(),
                Amount = 2000f,
                SalaryDate = DateOnly.FromDateTime(DateTime.Today),
                EmployeeId = empId
            };
            _repoMock
                .Setup(r => r.UpdateSalary(empId, updated))
                .ReturnsAsync(updated);

            var handler = new UpdateSalaryCommandHandler(_repoMock.Object);
            var command = new UpdateSalaryCommand(empId, updated);

            // Act
            var result = await handler.Handle(command, _ct);

            // Assert
            Assert.Equal(updated, result);
            _repoMock.Verify(r => r.UpdateSalary(empId, updated), Times.Once);
        }

        [Fact]
        public async Task UpdateSalaryCommandHandler_Should_Return_Null_If_Not_Found()
        {
            // Arrange
            var empId = Guid.NewGuid();
            var salary = new SalaryEntity();
            _repoMock
                .Setup(r => r.UpdateSalary(empId, salary))
                .ReturnsAsync((SalaryEntity?)null);

            var handler = new UpdateSalaryCommandHandler(_repoMock.Object);
            var command = new UpdateSalaryCommand(empId, salary);

            // Act
            var result = await handler.Handle(command, _ct);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteSalaryCommandHandler_Should_Call_DeleteSalaryByEmployeeId_And_Return_True()
        {
            // Arrange
            var empId = Guid.NewGuid();
            _repoMock
                .Setup(r => r.DeleteSalaryByEmployeeId(empId))
                .ReturnsAsync(true);

            var handler = new DeleteSalaryCommandHandler(_repoMock.Object);
            var command = new DeleteSalaryByEmpIdCommand(empId);

            // Act
            var result = await handler.Handle(command, _ct);

            // Assert
            Assert.True(result);
            _repoMock.Verify(r => r.DeleteSalaryByEmployeeId(empId), Times.Once);
        }

        [Fact]
        public async Task DeleteSalaryCommandHandler_Should_Return_False_If_Delete_Fails()
        {
            // Arrange
            var empId = Guid.NewGuid();
            _repoMock
                .Setup(r => r.DeleteSalaryByEmployeeId(empId))
                .ReturnsAsync(false);

            var handler = new DeleteSalaryCommandHandler(_repoMock.Object);
            var command = new DeleteSalaryByEmpIdCommand(empId);

            // Act
            var result = await handler.Handle(command, _ct);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetSalaryQueryHandler_Should_Return_All_Salaries()
        {
            // Arrange
            var list = new List<SalaryEntity>
            {
                new SalaryEntity { SalaryId = Guid.NewGuid(), Amount = 500f, SalaryDate = DateOnly.FromDateTime(DateTime.Today), EmployeeId = Guid.NewGuid() },
                new SalaryEntity { SalaryId = Guid.NewGuid(), Amount = 750f, SalaryDate = DateOnly.FromDateTime(DateTime.Today), EmployeeId = Guid.NewGuid() }
            };
            _repoMock
                .Setup(r => r.GetSalaries())
                .ReturnsAsync(list);

            var handler = new GetSalayQueryHandler(_repoMock.Object);
            var request = new GetSalaryQuery();
            var castHandler = handler as IRequestHandler<GetSalaryQuery, IEnumerable<SalaryEntity>>;

            // Act
            var result = await castHandler.Handle(request, _ct);

            // Assert
            Assert.Same(list, result);
            _repoMock.Verify(r => r.GetSalaries(), Times.Once);
        }

        [Fact]
        public async Task GetSalaryQueryHandler_Should_Handle_Empty_List()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetSalaries())
                .ReturnsAsync(new List<SalaryEntity>());

            var handler = new GetSalayQueryHandler(_repoMock.Object);
            var request = new GetSalaryQuery();
            var castHandler = handler as IRequestHandler<GetSalaryQuery, IEnumerable<SalaryEntity>>;

            // Act
            var result = await castHandler.Handle(request, _ct);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetSalariesByEmployeeIdQueryHandler_Should_Return_Salary_When_Found()
        {
            // Arrange
            var empId = Guid.NewGuid();
            var salary = new SalaryEntity { SalaryId = Guid.NewGuid(), Amount = 1200f, SalaryDate = DateOnly.FromDateTime(DateTime.Today), EmployeeId = empId };
            _repoMock
                .Setup(r => r.GetSalaryByEmployeeId(empId))
                .ReturnsAsync(salary);

            var handler = new GetSalariesByEmployeeIdQueryHandler(_repoMock.Object);
            var query = new GetSalariesByEmployeeIdQuery(empId);

            // Act
            var result = await handler.Handle(query, _ct);

            // Assert
            Assert.Equal(salary, result);
            _repoMock.Verify(r => r.GetSalaryByEmployeeId(empId), Times.Once);
        }

        [Fact]
        public async Task GetSalariesByEmployeeIdQueryHandler_Should_Return_Null_When_Not_Found()
        {
            // Arrange
            var empId = Guid.NewGuid();
            _repoMock
                .Setup(r => r.GetSalaryByEmployeeId(empId))
                .ReturnsAsync((SalaryEntity?)null);

            var handler = new GetSalariesByEmployeeIdQueryHandler(_repoMock.Object);
            var query = new GetSalariesByEmployeeIdQuery(empId);

            // Act
            var result = await handler.Handle(query, _ct);

            // Assert
            Assert.Null(result);
        }
    }
}