using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.Application.DTO;
using Employee.Core.Entities;
using Employee.Core.Enums;
using Employee.Core.Interfaces;
using MediatR;

namespace Employee.Application.Queries.Roles
{
    public record GetRolesByIdQuery(Guid EmployeeId):IRequest<RolesDTO>;
    public class GetRoleByIdQueryHandler(IRolesRepository rolesRepository)
        : IRequestHandler<GetRolesByIdQuery, RolesDTO>
    {
        public async Task<RolesDTO> Handle(GetRolesByIdQuery request, CancellationToken cancellationToken)
        {
            var roleDTO = new RolesDTO();
            var r = await rolesRepository.GetRolesById(request.EmployeeId);

            roleDTO.Permissions = Enum.GetName(typeof(Permissions), r.Permissions);
            roleDTO.RoleName = r.RoleName;
            roleDTO.Descriptions = r.Descriptions;
            roleDTO.EmployeeId = r.EmployeeId;

            return roleDTO;

        }
    }
}
