using System;
using System.Threading;
using System.Threading.Tasks;
using Employee.Application.Commands.RefreshToken;
using Employee.Core.Interfaces;
using Moq;
using Xunit;

namespace EmployeeXUnit.Test.ApplicationLayer
{
    public class RefreshTokenHandlerTests
    {
        private readonly Mock<IRefreshTokenRepository> _refreshRepoMock;
        private readonly CancellationToken _ct = CancellationToken.None;

        public RefreshTokenHandlerTests()
        {
            _refreshRepoMock = new Mock<IRefreshTokenRepository>();
        }

        [Fact]
        public async Task Handle_GivenValidToken_CallsRepositoryAndReturnsNewAccessToken()
        {
            // Arrange
            var inputToken = "old-refresh-token";
            var newAccessToken = "new-access-token";

            _refreshRepoMock
                .Setup(r => r.GetAccessToken(inputToken))
                .ReturnsAsync(newAccessToken);

            var handler = new RefreshTokenHandler(_refreshRepoMock.Object);
            var command = new RefreshTokenCommand(inputToken);

            // Act
            var result = await handler.Handle(command, _ct);

            // Assert
            Assert.Equal(newAccessToken, result);
            _refreshRepoMock.Verify(r => r.GetAccessToken(inputToken), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenRepositoryThrowsException_PropagatesException()
        {
            // Arrange
            var inputToken = "bad-token";
            var exception = new InvalidOperationException("Invalid token");

            _refreshRepoMock
                .Setup(r => r.GetAccessToken(inputToken))
                .ThrowsAsync(exception);

            var handler = new RefreshTokenHandler(_refreshRepoMock.Object);
            var command = new RefreshTokenCommand(inputToken);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, _ct));
            Assert.Same(exception, ex);
        }

        [Fact]
        public async Task Handle_WithEmptyToken_CallsRepositoryWithEmptyString()
        {
            // Arrange
            var inputToken = string.Empty;
            var expectedToken = "access-for-empty";

            _refreshRepoMock
                .Setup(r => r.GetAccessToken(inputToken))
                .ReturnsAsync(expectedToken);

            var handler = new RefreshTokenHandler(_refreshRepoMock.Object);
            var command = new RefreshTokenCommand(inputToken);

            // Act
            var result = await handler.Handle(command, _ct);

            // Assert
            Assert.Equal(expectedToken, result);
            _refreshRepoMock.Verify(r => r.GetAccessToken(inputToken), Times.Once);
        }
    }
}
