using Employee.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Application.Commands.Project
{
    public record DeleteProjectCommand(Guid Id) : IRequest<bool>;
    public class DeleteProjectCommandHandler(IProjectRepository projectRepository)
        : IRequestHandler<DeleteProjectCommand, bool>
    {
        public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            return await projectRepository.DeleteProject(request.Id);
        }
    }
}
