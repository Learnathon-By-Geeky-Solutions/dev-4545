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
    public class TasksController(ISender sender, IAuthorizationService authz) : ControllerBase
    {
        private readonly ISender _sender=sender;
        private readonly IAuthorizationService _authz = authz;



        // Admin and SE can view all tasks
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllTasks()
        {
            var result = await _sender.Send(new GetAllTasksQuery());
            return Ok(result);
        }

        // Admin and SE can view a specific task by employee ID
        [HttpGet("employees/{EmployeeId:guid}")]
        [Authorize(Roles = "Admin,SE")]
        public async Task<IActionResult> GetTaskByEmployeeId(Guid EmployeeId)
        {
            var authResult = await _authz.AuthorizeAsync(User, EmployeeId, "CanModifyOwnEmployee");
            if (!authResult.Succeeded)
                return Forbid();
            var result = await _sender.Send(new GetTaskByIdQuery(EmployeeId));
            return Ok(result);
        }
        [HttpGet("{Id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTaskById(Guid Id)
        {
            var result = await _sender.Send(new GetTaskByTaskIdQuery(Id));
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
            var forvalidation = await _sender.Send(new GetTaskByTaskIdQuery(id));
            if (forvalidation == null)
            {
                return BadRequest("Entity Not Found.");
            }
            var result = await _sender.Send(new UpdateTaskCommand(id, taskEntity));
            return result != null ? Ok(result) : BadRequest("Entity not found to update.");
        }

        // Only Admin can delete a task
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,SE")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var forvalidation = await _sender.Send(new GetTaskByTaskIdQuery(id));
            if(forvalidation == null)
            {
                return BadRequest("Entity Not Found.");
            }
            var authResult = await _authz.AuthorizeAsync(User, forvalidation!.EmployeeId, "CanModifyOwnEmployee");
            if (!authResult.Succeeded)
                return Forbid();
            var result = await _sender.Send(new DeleteTaskCommand(id));
            return Ok(result);
        }
    }
}
