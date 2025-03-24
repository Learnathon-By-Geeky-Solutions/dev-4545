using Employee.Core.Entities;
using Employee.Core.Interfaces;
using MediatR;

namespace Employee.Application.Commands.Roles
{
    public record UpdateRolesCommand(Guid RoleId, RolesEntity RolesEntity): IRequest<RolesEntity>;
    public class UpdateRolesCommandHandler(IRolesRepository rolesRepository)
        : IRequestHandler<UpdateRolesCommand, RolesEntity>
    {
        public async Task<RolesEntity> Handle(UpdateRolesCommand request, CancellationToken cancellationToken)
        {
            return await rolesRepository.UpdateRoles(request.RoleId, request.RolesEntity);
        }
    }
}
