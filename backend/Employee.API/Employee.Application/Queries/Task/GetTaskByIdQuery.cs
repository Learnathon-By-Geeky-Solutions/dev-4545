using Employee.Core.Entities;
using Employee.Core.Interfaces;
using MediatR;

namespace Employee.Application.Queries.Task
{
    public record GetTaskByIdQuery(Guid Id) : IRequest<IEnumerable<TaskEntity>>;
    public class GetTaskByIdQueryHandler(ITasksRepository taskRepository)
        : IRequestHandler<GetTaskByIdQuery,IEnumerable<TaskEntity>>
    {
        public async Task<IEnumerable< TaskEntity>> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            return await taskRepository.GetTaskByEmployeeIdAsync(request.Id);
        }
    }
}
