using Employee.Application.Commands.Project;
using Employee.Application.Commands.Task;
using Employee.Application.Queries.Project;
using Employee.Application.Queries.Task;
using Employee.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Employee.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController(ISender sender): ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllProjects()
        {
            var result = await sender.Send(new GetAllProjectsQuery());
            return Ok(result);
        }
        [Authorize(Roles = "Admin,SE")]
        [HttpGet("employees/{EmployeeId:guid}")]
        public async Task<IActionResult> GetProjectByEmployeeId(Guid EmployeeId)
        {
            var result = await sender.Send(new GetProjectByEmployeeIdQuery(EmployeeId));
            return Ok(result);
        }
        [Authorize(Roles = "Admin,SE")]
        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetProjectById(Guid Id)
        {
            var result = await sender.Send(new GetProjectByIdQuery(Id));
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProjectAsync([FromBody] ProjectEntity Project)
        {
            var result = await sender.Send(new AddProjectCommand(Project));
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "Admin,SE")]
        public async Task<IActionResult> UpdateProject(Guid Id, ProjectEntity Project)
        {
            var result = await sender.Send(new UpdateProjectCommand(Id, Project));
            if(result!=null)
                return Ok(result);
            return BadRequest("Not found the entity to update");
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProject(Guid Id)
        {
            var result = await sender.Send(new DeleteProjectCommand(Id));
            return Ok(result);
        }
    }
}
