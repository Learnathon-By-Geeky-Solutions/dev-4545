using Employee.Core.Entities;
using Employee.Core.Interfaces;
using MediatR;

namespace Employee.Application.Queries.Task
{
    public record GetTaskByIdQuery(Guid Id) : IRequest<TaskEntity>;
    public class GetTaskByIdCommandHandler(ITaskRepository taskRepository)
        : IRequestHandler<GetTaskByIdQuery, TaskEntity>
    {
        public async Task<TaskEntity> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            return await taskRepository.GetTaskByIdAsync(request.Id);
        }
    }
}
