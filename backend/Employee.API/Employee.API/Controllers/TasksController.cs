using System;
using System.Threading.Tasks;
using Employee.Application.Commands.Task;
using Employee.Application.Queries.Task;
using Employee.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Employee.API.Controllers
{
    [Authorize] // Require authenticated user for all actions by default
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ISender _sender;

        public TasksController(ISender sender)
        {
            _sender = sender;
        }

        // Admin and SE can view all tasks
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllTasks()
        {
            var result = await _sender.Send(new GetAllTasksQuery());
            return Ok(result);
        }

        // Admin and SE can view a specific task by employee ID
        [HttpGet("{employeeId}")]
        [Authorize(Roles = "Admin,SE")]
        public async Task<IActionResult> GetTaskById(Guid employeeId)
        {
            var result = await _sender.Send(new GetTaskByIdQuery(employeeId));
            return Ok(result);
        }

        // Only Admin can create a new task
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddTaskAsync([FromBody] TaskEntity taskEntity)
        {
            var result = await _sender.Send(new AddTaskCommand(taskEntity));
            return Ok(result);
        }

        // Only Admin can update an existing task
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,SE")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] TaskEntity taskEntity)
        {
            var result = await _sender.Send(new UpdateTaskCommand(id, taskEntity));
            return result != null ? Ok(result) : BadRequest("Entity not found to update.");
        }

        // Only Admin can delete a task
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,SE")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var result = await _sender.Send(new DeleteTaskCommand(id));
            return Ok(result);
        }
    }
}
