using Employee.Core.Entities;
using Employee.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Application.Queries.Project
{
    public record GetProjectByIdQuery(Guid EmployeeId) : IRequest<IEnumerable<ProjectEntity>>;

    public class GetProjectByIdQueryHandler(IProjectRepository projectRepository)
        : IRequestHandler<GetProjectByIdQuery, IEnumerable<ProjectEntity>>
    {
        public async Task<IEnumerable<ProjectEntity>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            return await projectRepository.GetProjectByEmployeeId(request.EmployeeId);
        }
    }
}
    
