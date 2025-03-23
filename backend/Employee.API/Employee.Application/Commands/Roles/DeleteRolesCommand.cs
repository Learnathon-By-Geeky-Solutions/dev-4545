using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.Core.Interfaces;
using MediatR;

namespace Employee.Application.Commands.Roles
{
    public record DeleteRolesCommand(Guid RoleId): IRequest<bool>;
    public class DeleteRolesCommandHandler(IRolesRepository rolesRepository)
        : IRequestHandler<DeleteRolesCommand, bool>
    {
        public async Task<bool> Handle(DeleteRolesCommand request, CancellationToken cancellationToken)
        {
            return await rolesRepository.DeleteRolesById(request.RoleId);
        }
    }
}
