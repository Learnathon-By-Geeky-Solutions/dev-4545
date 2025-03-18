using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using MediatR;

namespace Employee.Application.Queries.Task
{
    public record GetTaskByIdCommand(Guid Id) : IRequest<TaskEntity>;
    public class GetTaskByIdCommandHandler(ITaskRepository taskRepository)
        : IRequestHandler<GetTaskByIdCommand, TaskEntity>
    {
        public async Task<TaskEntity> Handle(GetTaskByIdCommand request, CancellationToken cancellationToken)
        {
            return await taskRepository.GetTaskByIdAsync(request.Id);
        }
    }
}
