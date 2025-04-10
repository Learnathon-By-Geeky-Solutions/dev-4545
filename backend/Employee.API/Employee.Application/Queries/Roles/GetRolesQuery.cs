using Employee.Application.DTO;
using Employee.Core.Entities;
using Employee.Core.Enums;
using Employee.Core.Interfaces;
using MediatR;

namespace Employee.Application.Queries.Roles
{
    public record GetRolesQuery():IRequest<IEnumerable<RolesDTO>>;
    public class GetRolesQueryHandler(IRolesRepository rolesRepository)
        : IRequestHandler<GetRolesQuery, IEnumerable<RolesDTO>>
    {
        public async Task<IEnumerable<RolesDTO>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<RolesEntity> result = await rolesRepository.GetRoles();
            var rolesDTO =new List<RolesDTO>();
            foreach (var r in result)
            {
                RolesDTO roleDTO = new RolesDTO();
                roleDTO.Permissions = Enum.GetName(typeof(Permissions), r.Permissions);
                roleDTO.RoleName = r.RoleName;
                roleDTO.Descriptions = r.Descriptions;
                rolesDTO.Add(roleDTO);
            }
            return rolesDTO;
        }
    }
}
