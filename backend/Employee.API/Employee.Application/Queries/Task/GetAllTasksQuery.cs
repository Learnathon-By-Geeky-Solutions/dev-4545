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
    public record GetAllTasksQuery : IRequest<IEnumerable<TaskEntity>>;

    public class GetAllTasksQueryHandler(ITaskRepository taskRepository)
        : IRequestHandler<GetAllTasksQuery, IEnumerable<TaskEntity>>
    {
        public async Task<IEnumerable<TaskEntity>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            return await taskRepository.GetAllTasks();

        }
    }
}
