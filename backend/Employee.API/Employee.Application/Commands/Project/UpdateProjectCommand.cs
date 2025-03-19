using Employee.Core.Entities;
using Employee.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Application.Commands.Project
{
    public record UpdateProjectCommand(Guid Id, ProjectEntity Project) : IRequest<ProjectEntity>;
    public class UpdateProjectCommandHandler(IProjectRepository projectRepository)
        : IRequestHandler<UpdateProjectCommand, ProjectEntity>
    {
        public async Task<ProjectEntity> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            return await projectRepository.UpdateProject(request.Id, request.Project);
        }
    }
}
