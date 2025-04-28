using Employee.Core.Entities;
using Employee.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Application.Queries.Task
{
    public record GetTaskByTaskIdQuery(Guid Id) : IRequest<TaskEntity?>;

    public class GetProjectByIdHandler(ITasksRepository tasksRepository)
        : IRequestHandler<GetTaskByTaskIdQuery, TaskEntity?>
    {
        public async Task<TaskEntity?> Handle(GetTaskByTaskIdQuery request, CancellationToken cancellationToken)
        {
            return await tasksRepository.GetTaskByTaskId(request.Id);
        }
    }
}

