using Employee.Application.Commands.Task;
using Employee.Application.Queries.Task;
using Employee.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Employee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController(ISender sender): ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var result = await sender.Send(new GetAllTasksQuery());
            return Ok(result);
        }
        [HttpGet("task")]
        public async Task<IActionResult> GetTaskById(Guid Id)
        {
            var result = await sender.Send(new GetTaskByIdCommand(Id));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddTaskAsync([FromBody] TaskEntity taskEntity)
        {
            var result = await sender.Send(new AddTaskCommand(taskEntity));
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTask(Guid Id, TaskEntity taskEntity)
        {
            var result = await sender.Send(new UpdateTaskCommand(Id, taskEntity));
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTask(Guid Id)
        {
            var result = await sender.Send(new DeleteTaskCommand(Id));
            return Ok(result);
        }
    }
}
