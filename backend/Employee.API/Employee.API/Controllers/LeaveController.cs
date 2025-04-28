using Employee.Application.Commands.Leave;
using Employee.Application.Queries.Leave;
using Employee.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Employee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LeaveController(ISender sender, IAuthorizationService authz) : ControllerBase
    {
        private readonly ISender _sender = sender;
        private readonly IAuthorizationService _authz = authz;

        private async Task<IActionResult> AuthorizeAndExecuteAsync(Guid employeeId, Func<Task<IActionResult>> action)
        {
            var authResult = await _authz.AuthorizeAsync(User, employeeId, "CanModifyOwnEmployee");
            return authResult.Succeeded ? await action() : Forbid();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SE")]
        public Task<IActionResult> InsertLeave(Guid employeeId, LeaveEntity leave) =>
            AuthorizeAndExecuteAsync(employeeId, async () =>
            {
                var result = await _sender.Send(new AddLeaveCommand(leave));
                return Ok(result);
            });

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetLeaves()
        {
            var result = await _sender.Send(new GetLeaveQuery());
            return Ok(result);
        }

        [HttpGet("GetLeaveByEmployeeId")]
        [Authorize(Roles = "Admin,SE")]
        public Task<IActionResult> GetLeaveByEmpId(Guid employeeId) =>
            AuthorizeAndExecuteAsync(employeeId, async () =>
            {
                var result = await _sender.Send(new GetLeavesByEmployeeIdQuery(employeeId));
                return Ok(result);
            });

        [HttpPut]
        [Authorize(Roles = "Admin,SE")]
        public Task<IActionResult> UpdateLeaveByEmpId(Guid employeeId, LeaveEntity leaveEntity) =>
            AuthorizeAndExecuteAsync(employeeId, async () =>
            {
                var result = await _sender.Send(new UpdateLeaveCommand(employeeId, leaveEntity));
                return result == null
                    ? BadRequest("Not found the entity to update.")
                    : Ok(result);
            });

        [HttpDelete]
        [Authorize(Roles = "Admin,SE")]
        public Task<IActionResult> DeleteLeaveByEmpId(Guid employeeId) =>
            AuthorizeAndExecuteAsync(employeeId, async () =>
            {
                var result = await _sender.Send(new DeleteLeaveByEmpIdCommand(employeeId));
                return Ok(result);
            });
    }
}
