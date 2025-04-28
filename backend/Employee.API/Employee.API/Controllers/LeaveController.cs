using Employee.Application.Commands.Leave;
using Employee.Application.Queries.Leave;
using Employee.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Employee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LeaveController(ISender sender, IAuthorizationService authz) : ControllerBase
    {
        private readonly IAuthorizationService _authz = authz;

        [HttpPost]
        [Authorize(Roles = "Admin,SE")]
        public async Task<IActionResult> InsertLeave(Guid EmployeeId,LeaveEntity Leave)
        {
            var authResult = await _authz.AuthorizeAsync(User, EmployeeId, "CanModifyOwnEmployee");
            if (!authResult.Succeeded)
                return Forbid();
            var result = await sender.Send(new AddLeaveCommand(Leave));
            return Ok(result);

        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetLeaves()
        {
            var result = await sender.Send(new GetLeaveQuery());
            return Ok(result);

        }
        [Authorize(Roles = "Admin,SE")]
        [HttpGet("GetLeaveByEmployeeId")]
        public async Task<IActionResult> GetLeaveByEmpId(Guid EmployeeId)
        {
            var authResult = await _authz.AuthorizeAsync(User, EmployeeId, "CanModifyOwnEmployee");
            if (!authResult.Succeeded)
                return Forbid();
            var result = await sender.Send(new GetLeavesByEmployeeIdQuery(EmployeeId));
            return Ok(result);

        }
        [Authorize(Roles = "Admin,SE")]
        [HttpPut]
        public async Task<IActionResult> UpdateLeaveByEmpId(Guid EmployeeId, LeaveEntity LeaveEntity)
        {
            var authResult = await _authz.AuthorizeAsync(User, EmployeeId, "CanModifyOwnEmployee");
            if (!authResult.Succeeded)
                return Forbid();
            var result = await sender.Send(new UpdateLeaveCommand(EmployeeId, LeaveEntity));
            if (result == null)
            {
                return BadRequest("Not found the entity to update.");
            }
            return Ok(result);

        }
        [Authorize(Roles = "Admin,SE")]
        [HttpDelete]
        public async Task<IActionResult> DeleteLeaveByEmpId(Guid EmployeeId)
        {
            var authResult = await _authz.AuthorizeAsync(User, EmployeeId, "CanModifyOwnEmployee");
            if (!authResult.Succeeded)
                return Forbid();
            var result = await sender.Send(new DeleteLeaveByEmpIdCommand(EmployeeId));
            return Ok(result);

        }
    }
}
