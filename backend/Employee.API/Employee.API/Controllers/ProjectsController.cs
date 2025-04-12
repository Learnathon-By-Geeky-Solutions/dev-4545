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
   // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController(ISender sender): ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var result = await sender.Send(new GetAllProjectsQuery());
            return Ok(result);
        }
        [HttpGet("EmployeeId")]
        public async Task<IActionResult> GetProjectById(Guid EmployeeId)
        {
            var result = await sender.Send(new GetProjectByIdQuery(EmployeeId));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddProjectAsync([FromBody] ProjectEntity Project)
        {
            var result = await sender.Send(new AddProjectCommand(Project));
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProject(Guid Id, ProjectEntity Project)
        {
            var result = await sender.Send(new UpdateProjectCommand(Id, Project));
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProject(Guid Id)
        {
            var result = await sender.Send(new DeleteProjectCommand(Id));
            return Ok(result);
        }
    }
}
