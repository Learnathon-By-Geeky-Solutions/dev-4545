using Employee.API.Controllers;
using Employee.Application.Commands.Roles;
using Employee.Application.DTO;
using Employee.Application.Queries.Roles;
using Employee.Core.Entities;
using Employee.Core.Enums;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeXUnit.Test.PresentationLayer.Controllers
{
    public class RolesControllerTests
    {
        private readonly Mock<ISender> _senderMock;
        private readonly RolesController _controller;

        public RolesControllerTests()
        {
            _senderMock = new Mock<ISender>();
            _controller = new RolesController(_senderMock.Object);
        }

        [Fact]
        public async Task InsertRole_ShouldReturnOkResult()
        {
            // Arrange
            var role = new RolesEntity
            {
                RoleId = Guid.NewGuid(),
                RoleName = "Admin",
                Descriptions = "Administrator role with full permissions",
                Permissions = Permissions.Admin // Assuming Permissions is an Enum
            };
            _senderMock.Setup(s => s.Send(It.IsAny<AddRolesCommand>(), default))
                       .ReturnsAsync(role);

            // Act
            var result = await _controller.InsertRole(role);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeEquivalentTo(role);
        }

        [Fact]
        public async Task GetRoles_ShouldReturnOkResult()
        {
            // Arrange
            var roles = new List<RolesEntity>
            {
                new RolesEntity
                {
                    RoleId = Guid.NewGuid(),
                    RoleName = "Admin",
                    Descriptions = "Administrator role with full permissions",
                    Permissions = Permissions.Admin
                }
            };
            var resp = new List<RolesDTO>
            {
                new RolesDTO
                {
                
                    RoleName = "Admin",
                    Descriptions = "Administrator role with full permissions",
                    Permissions = Enum.GetName(typeof(Permissions), Permissions.Admin),
                }

            };
            _senderMock.Setup(s => s.Send(It.IsAny<GetRolesQuery>(), default))
                       .ReturnsAsync(resp);

            // Act
            var result = await _controller.GetRoles();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeEquivalentTo(resp);
        }

        [Fact]
        public async Task GetRoleById_ShouldReturnOkResult_WhenRoleExists()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            var role = new RolesEntity
            {
                RoleId = roleId,
                RoleName = "Admin",
                Descriptions = "Administrator role with full permissions",
                Permissions = Permissions.Admin
            };
            _senderMock.Setup(s => s.Send(It.Is<GetRolesByIdQuery>(q => q.RoleId == roleId), default))
                       .ReturnsAsync(role);

            // Act
            var result = await _controller.GetRoleById(roleId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeEquivalentTo(role);
        }

        [Fact]
        public async Task UpdateRoles_ShouldReturnOkResult_WhenRoleExists()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            var role = new RolesEntity
            {
                RoleId = roleId,
                RoleName = "Admin Updated",
                Descriptions = "Updated description",
                Permissions = Permissions.Admin
            };
            _senderMock.Setup(s => s.Send(It.IsAny<UpdateRolesCommand>(), default))
                       .ReturnsAsync(role);

            // Act
            var result = await _controller.UpdateRoles(roleId, role);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            okResult.Value.Should().BeEquivalentTo(role);
        }

        [Fact]
        public async Task UpdateRoles_ShouldReturnBadRequest_WhenRoleNotFound()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            var role = new RolesEntity
            {
                RoleId = roleId,
                RoleName = "Non-Existent Role",
                Descriptions = "This role does not exist",
                Permissions = Permissions.CEO,
            };
            _senderMock.Setup(s => s.Send(It.IsAny<UpdateRolesCommand>(), default))
                       .ReturnsAsync((RolesEntity)null);

            // Act
            var result = await _controller.UpdateRoles(roleId, role);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            badRequestResult.Value.Should().Be("Entity Not Found to Update.");
        }

        [Fact]
        public async Task DeleteRole_ShouldReturnOkResult()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            _senderMock.Setup(s => s.Send(It.IsAny<DeleteRolesCommand>(), default))
                       .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteRole(roleId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
