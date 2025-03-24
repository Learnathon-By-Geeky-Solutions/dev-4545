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
    public record GetAllProjectsQuery : IRequest<IEnumerable<ProjectEntity>>;

    public class GetAllProjectsQueryHandler(IProjectRepository projectRepository)
        : IRequestHandler<GetAllProjectsQuery, IEnumerable<ProjectEntity>>
    {
        public async Task<IEnumerable<ProjectEntity>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            return await projectRepository.GetAllProjects();
        }
    }
}
