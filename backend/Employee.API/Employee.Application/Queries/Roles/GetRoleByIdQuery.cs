using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using MediatR;

namespace Employee.Application.Queries.Roles
{
    public record GetRolesByIdQuery(Guid RoleId):IRequest<RolesEntity>;
    public class GetRoleByIdQueryHandler(IRolesRepository rolesRepository)
        : IRequestHandler<GetRolesByIdQuery, RolesEntity>
    {
        public async Task<RolesEntity> Handle(GetRolesByIdQuery request, CancellationToken cancellationToken)
        {
            return await rolesRepository.GetRolesById(request.RoleId);

        }
    }
}
