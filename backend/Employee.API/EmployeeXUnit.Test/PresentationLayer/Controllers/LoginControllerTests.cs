using Employee.API.Controllers;
using Employee.Application.Commands.Login;
using Management.Core.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeXUnit.Test.PresentationLayer.Controllers
{
    public class LoginControllerTests
    {
        private readonly Mock<ISender> _mockSender;
        private readonly LoginController _controller;

        public LoginControllerTests()
        {
            _mockSender = new Mock<ISender>();
            _controller = new LoginController(_mockSender.Object);
        }

        [Fact]
        public async Task Authentication_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var authRequest = new AuthenticationRequest
            {
                Email = "test@example.com",
                Password = "password123"
            };

            var authResponse = new AuthenticationResponse
            {
                Id = Guid.NewGuid(),
                JwToken = "some-jwt-token",
                Role = "Admin",
                RefreshToken = "some-refresh-token"
            };

            // Setup the sender mock to return a valid response
            _mockSender.Setup(sender => sender.Send(It.IsAny<LoginCommand>(), default))
                .ReturnsAsync(authResponse);

            // Act
            var result = await _controller.Authentication(authRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<AuthenticationResponse>(okResult.Value);

            Assert.Equal(authResponse.Id, response.Id);
            Assert.Equal(authResponse.JwToken, response.JwToken);
            Assert.Equal(authResponse.Role, response.Role);
            Assert.Equal(authResponse.RefreshToken, response.RefreshToken);
        }

        

    }
}
