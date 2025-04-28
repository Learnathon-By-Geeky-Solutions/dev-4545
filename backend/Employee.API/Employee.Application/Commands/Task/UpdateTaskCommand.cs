using Employee.Core.Entities;
using Employee.Core.Interfaces;
using MediatR;

namespace Employee.Application.Commands.Task
{
    public record UpdateTaskCommand(Guid Id, TaskEntity TaskEntity) : IRequest<TaskEntity?>;
    public class UpdateTaskCommandHandler(ITasksRepository taskRepository)
        : IRequestHandler<UpdateTaskCommand, TaskEntity?>
    {
        public async Task<TaskEntity?> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            return await taskRepository.UpdateTask(request.Id, request.TaskEntity);
        }
    }
}
