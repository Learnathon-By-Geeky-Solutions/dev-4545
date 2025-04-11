using Employee.Core.Interfaces;
using MediatR;

namespace Employee.Application.Commands.Task
{
    public record DeleteTaskCommand(Guid Id) : IRequest<bool>;
    public class DeleteTaskCommandHandler(ITaskRepository taskRepository)
        : IRequestHandler<DeleteTaskCommand, bool>
    {
        public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            return await taskRepository.DeleteTask(request.Id);
        }
    }
}
