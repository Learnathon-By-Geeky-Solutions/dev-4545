using Employee.Application.Commands.Performance;
using Employee.Core.Interfaces;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EmployeeXUnit.Test.ApplicationLayer.Commands.Performance
{
    public class DeletePerformanceCommandTests
    {
        [Fact]
        public async Task Handle_ShouldReturnTrue_WhenDeleted()
        {
            var id = Guid.NewGuid();
            var mockRepo = new Mock<IPerformanceRepository>();
            mockRepo.Setup(x => x.DeletePerformance(id)).ReturnsAsync(true);

            var handler = new DeletePerformanceCommandHandler(mockRepo.Object);
            var command = new DeletePerformanceCommand(id);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result);
            mockRepo.Verify(x => x.DeletePerformance(id), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFalse_WhenNotDeleted()
        {
            var id = Guid.NewGuid();
            var mockRepo = new Mock<IPerformanceRepository>();
            mockRepo.Setup(x => x.DeletePerformance(id)).ReturnsAsync(false);

            var handler = new DeletePerformanceCommandHandler(mockRepo.Object);
            var command = new DeletePerformanceCommand(id);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryFails()
        {
            var id = Guid.NewGuid();
            var mockRepo = new Mock<IPerformanceRepository>();
            mockRepo.Setup(x => x.DeletePerformance(id)).ThrowsAsync(new Exception("DB Failure"));

            var handler = new DeletePerformanceCommandHandler(mockRepo.Object);

            await Assert.ThrowsAsync<Exception>(() =>
                handler.Handle(new DeletePerformanceCommand(id), CancellationToken.None));
        }
    }
}