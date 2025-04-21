using Employee.Application.Queries.Leave;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using MediatR;
using Moq;

namespace EmployeeXUnit.Test.ApplicationLayer.Queries.Leave
{
    public class GetLeaveQueryTests
    {
        [Fact]
        public async Task Handle_ShouldReturnListOfLeaves()
        {
            // Arrange
            var mockRepo = new Mock<ILeaveRepository>();
            var expectedLeaves = new List<LeaveEntity>
        {
            new LeaveEntity { LeaveId = Guid.NewGuid(), Reason = "Vacation" },
            new LeaveEntity { LeaveId = Guid.NewGuid(), Reason = "Sick Leave" }
        };

            mockRepo.Setup(repo => repo.GetLeaves()).ReturnsAsync(expectedLeaves);

            var handler = new GetLeaveQueryHanlder(mockRepo.Object);
            var query = new GetLeaveQuery();

            // Act
            var result = await ((IRequestHandler<GetLeaveQuery, IEnumerable<LeaveEntity>>)handler)
                .Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            mockRepo.Verify(repo => repo.GetLeaves(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoLeavesExist()
        {
            // Arrange
            var mockRepo = new Mock<ILeaveRepository>();
            mockRepo.Setup(repo => repo.GetLeaves()).ReturnsAsync(new List<LeaveEntity>());

            var handler = new GetLeaveQueryHanlder(mockRepo.Object);
            var query = new GetLeaveQuery();

            // Act
            var result = await ((IRequestHandler<GetLeaveQuery, IEnumerable<LeaveEntity>>)handler)
                .Handle(query, CancellationToken.None);

            // Assert
            Assert.Empty(result);
            mockRepo.Verify(repo => repo.GetLeaves(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryFails()
        {
            // Arrange
            var mockRepo = new Mock<ILeaveRepository>();
            mockRepo.Setup(repo => repo.GetLeaves()).ThrowsAsync(new Exception("Database failure"));

            var handler = new GetLeaveQueryHanlder(mockRepo.Object);
            var query = new GetLeaveQuery();

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                await ((IRequestHandler<GetLeaveQuery, IEnumerable<LeaveEntity>>)handler)
                    .Handle(query, CancellationToken.None);
            });

            mockRepo.Verify(repo => repo.GetLeaves(), Times.Once);
        }
    }
}