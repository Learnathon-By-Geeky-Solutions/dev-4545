using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employee.Core.Entities;
using Employee.Core.Interfaces;
using MediatR;

namespace Employee.Application.Commands.Task
{
    public record AddTaskCommand(TaskEntity TaskEntity) : IRequest<TaskEntity>;
    public class AddTaskCommandHandler(ITaskRepository taskRepository)
        : IRequestHandler<AddTaskCommand, TaskEntity>
    {
        public async Task<TaskEntity> Handle(AddTaskCommand request, CancellationToken cancellationToken)
        {
            return await taskRepository.AddTaskAsync(request.TaskEntity);
        }
    }
}
