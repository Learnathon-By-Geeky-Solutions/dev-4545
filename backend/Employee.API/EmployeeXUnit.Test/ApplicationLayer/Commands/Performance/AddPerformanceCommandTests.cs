using Employee.Application.Commands.Performance;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using Moq;

namespace EmployeeXUnit.Test.ApplicationLayer.Commands.Performance
{
    public class AddPerformanceCommandTests
    {
        [Fact]
        public async Task Handle_ShouldReturnPerformance_WhenAddedSuccessfully()
        {
            var performance = new PerformanceEntity { Id = Guid.NewGuid(), EmployeeId = Guid.NewGuid(), Rating = "Average" };
            var mockRepo = new Mock<IPerformanceRepository>();
            mockRepo.Setup(x => x.AddPerformanceAsync(performance)).ReturnsAsync(performance);

            var handler = new AddPerformanceCommandHandler(mockRepo.Object);
            var command = new AddPerformanceCommand(performance);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(performance.Id, result.Id);
            mockRepo.Verify(x => x.AddPerformanceAsync(performance), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryFails()
        {
            var performance = new PerformanceEntity();
            var mockRepo = new Mock<IPerformanceRepository>();
            mockRepo.Setup(x => x.AddPerformanceAsync(performance)).ThrowsAsync(new Exception("DB Error"));

            var handler = new AddPerformanceCommandHandler(mockRepo.Object);

            await Assert.ThrowsAsync<Exception>(() =>
                handler.Handle(new AddPerformanceCommand(performance), CancellationToken.None));
        }
    }
}