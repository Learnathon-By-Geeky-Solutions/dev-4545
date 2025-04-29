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
    public record GetProjectByIdQuery(Guid Id) : IRequest<ProjectEntity?>;

    public class GetProjectByIdQueryHandler(IProjectRepository projectRepository)
        : IRequestHandler<GetProjectByIdQuery, ProjectEntity?>
    {
        public async Task<ProjectEntity?> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            return await projectRepository.GetProjectById(request.Id);
        }
    }
}

