using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using MediatR;

namespace Employee.Application.Commands.Roles
{
    public record AddRolesCommand(RolesEntity RolesEntity): IRequest<RolesEntity>;
    public class AddRolesCommandHandler(IRolesRepository rolesRepository)
        : IRequestHandler<AddRolesCommand, RolesEntity>
    {
        public async Task<RolesEntity> Handle(AddRolesCommand request, CancellationToken cancellationToken)
        {
            return await rolesRepository.AddRoles(request.RolesEntity);
        }
    }
}
