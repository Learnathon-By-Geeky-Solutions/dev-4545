using Employee.Application.Commands.Task;
using Employee.Application.Queries.Task;
using Employee.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Employee.API.Controllers
{
    [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin,SE")]
        [HttpGet("EmployeeId")]
        public async Task<IActionResult> GetTaskById(Guid EmployeeId)
        {
            var result = await sender.Send(new GetTaskByIdCommand(EmployeeId));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddTaskAsync([FromBody] TaskEntity taskEntity)
        {
            var result = await sender.Send(new AddTaskCommand(taskEntity));
            return Ok(result);
        }
        [Authorize(Roles = "Admin,SE")]
        [HttpPut]
        public async Task<IActionResult> UpdateTask(Guid Id, TaskEntity taskEntity)
        {
            var result = await sender.Send(new UpdateTaskCommand(Id, taskEntity));
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Entity Not Found to Update.");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTask(Guid Id)
        {
            var result = await sender.Send(new DeleteTaskCommand(Id));
            return Ok(result);
        }
    }
}
