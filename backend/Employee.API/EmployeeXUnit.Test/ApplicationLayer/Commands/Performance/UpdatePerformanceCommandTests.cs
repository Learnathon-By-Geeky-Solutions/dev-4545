using Employee.Application.Commands.Performance;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using Moq;

namespace EmployeeXUnit.Test.ApplicationLayer.Commands.Performance
{
    public class UpdatePerformanceCommandTests
    {
        [Fact]
        public async Task Handle_ShouldReturnUpdatedPerformance()
        {
            var id = Guid.NewGuid();
            var updated = new PerformanceEntity { Id = id, EmployeeId = Guid.NewGuid(), Rating = "Medium" };

            var mockRepo = new Mock<IPerformanceRepository>();
            mockRepo.Setup(x => x.UpdatePerformance(id, updated)).ReturnsAsync(updated);

            var handler = new UpdatePerformanceCommandHandler(mockRepo.Object);
            var command = new UpdatePerformanceCommand(id, updated);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            mockRepo.Verify(x => x.UpdatePerformance(id, updated), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenNotFound()
        {
            var id = Guid.NewGuid();
            var input = new PerformanceEntity { Id = id };

            var mockRepo = new Mock<IPerformanceRepository>();
            mockRepo.Setup(x => x.UpdatePerformance(id, input)).ReturnsAsync((PerformanceEntity?)null);

            var handler = new UpdatePerformanceCommandHandler(mockRepo.Object);
            var command = new UpdatePerformanceCommand(id, input);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Null(result);
            mockRepo.Verify(x => x.UpdatePerformance(id, input), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryFails()
        {
            var id = Guid.NewGuid();
            var input = new PerformanceEntity();

            var mockRepo = new Mock<IPerformanceRepository>();
            mockRepo.Setup(x => x.UpdatePerformance(id, input)).ThrowsAsync(new Exception("Update error"));

            var handler = new UpdatePerformanceCommandHandler(mockRepo.Object);
            var command = new UpdatePerformanceCommand(id, input);

            await Assert.ThrowsAsync<Exception>(() =>
                handler.Handle(command, CancellationToken.None));
        }
    }
}