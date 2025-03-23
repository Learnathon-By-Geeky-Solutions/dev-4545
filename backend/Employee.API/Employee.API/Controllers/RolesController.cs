using Employee.Application.Commands.Roles;
using Employee.Application.Queries.Roles;
using Employee.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Employee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> InsertRole(RolesEntity rolesEntity)
        {
            var result = await sender.Send(new AddRolesCommand(rolesEntity));
            return Ok(result);

        }
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var result = await sender.Send(new GetRolesQuery());
            return Ok(result);

        }
        [HttpGet("GetRoleById")]
        public async Task<IActionResult> GetRoleById(Guid RolesId)
        {
            var result = await sender.Send(new GetRolesByIdQuery(RolesId));
            return Ok(result);

        }

        [HttpPut]
        public async Task<IActionResult> UpdateRoles(Guid RoleId, RolesEntity rolesEntity)
        {
            var result = await sender.Send(new UpdateRolesCommand(RoleId, rolesEntity));
            if (result == null)
            {
                return BadRequest("Entity Not Found to Update.");
            }
            return Ok(result);

        }
        [HttpDelete]
        public async Task<IActionResult> DeleteRole(Guid RoleId)
        {
            var result = await sender.Send(new DeleteRolesCommand(RoleId));
            return Ok(result);

        }
    }
}
