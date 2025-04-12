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
    public record GetTaskByIdCommand(Guid EmployeeId) : IRequest<IEnumerable<TaskEntity>>;
    public class GetTaskByIdCommandHandler(ITaskRepository taskRepository)
        : IRequestHandler<GetTaskByIdCommand, IEnumerable<TaskEntity>>
    {
        public async Task<IEnumerable<TaskEntity>> Handle(GetTaskByIdCommand request, CancellationToken cancellationToken)
        {
            return await taskRepository.GetTaskByEmployeeIdAsync(request.EmployeeId);
        }
    }
}
