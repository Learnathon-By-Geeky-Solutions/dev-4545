using Employee.Application.Queries.Task;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using Moq;

namespace EmployeeXUnit.Test.PresentationLayer.Controllers
{
    public class GetTaskByIdQueryHandlerTests
    {
        private readonly Mock<ITasksRepository> _mockRepo;
        private readonly GetTaskByIdQueryHandler _handler;

        public GetTaskByIdQueryHandlerTests()
        {
            _mockRepo = new Mock<ITasksRepository>();
            _handler = new GetTaskByIdQueryHandler(_mockRepo.Object);
        }

        [Fact]
        public async Task Handle_ValidEmployeeId_ReturnsTaskList()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var expectedTasks = new List<TaskEntity>
        {
            new TaskEntity
            {
                TaskId = Guid.NewGuid(),
                Description = "Fix login bug",
                AssignedDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(5),
                Status = "Pending",
                AssignedBy = Guid.NewGuid(),
                EmployeeId = employeeId,
                FeatureId = Guid.NewGuid()
            }
        };

            _mockRepo.Setup(r => r.GetTaskByEmployeeIdAsync(employeeId))
                     .ReturnsAsync(expectedTasks);

            var query = new GetTaskByIdQuery(employeeId);

            // Act
            var result = await _handler.Handle(query, default);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Fix login bug", result.First().Description);
        }

        [Fact]
        public async Task Handle_NoTasksForEmployee_ReturnsEmptyList()
        {
            // Arrange
            var employeeId = Guid.NewGuid();

            _mockRepo.Setup(r => r.GetTaskByEmployeeIdAsync(employeeId))
                     .ReturnsAsync(new List<TaskEntity>());

            var query = new GetTaskByIdQuery(employeeId);

            // Act
            var result = await _handler.Handle(query, default);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}