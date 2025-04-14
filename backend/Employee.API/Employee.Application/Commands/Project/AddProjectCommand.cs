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
    public record AddProjectCommand(ProjectEntity Project): IRequest<ProjectEntity>;
    public class AddProjectCommandHandler(IProjectRepository projectRepository)
        : IRequestHandler<AddProjectCommand, ProjectEntity>
    {
        public async Task<ProjectEntity> Handle(AddProjectCommand request, CancellationToken cancellationToken)
        {
            return await projectRepository.AddProjectAsync(request.Project);
        }
    }
}
