using Employee.Application.Commands.Leave;
using Employee.Application.Queries.Leave;
using Employee.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Employee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> InsertLeave(LeaveEntity Leave)
        {
            var result = await sender.Send(new AddLeaveCommand(Leave));
            return Ok(result);

        }
        [HttpGet]
        public async Task<IActionResult> GetLeaves()
        {
            var result = await sender.Send(new GetLeaveQuery());
            return Ok(result);

        }
        [HttpGet("GetLeaveByEmployeeId")]
        public async Task<IActionResult> GetLeaveByEmpId(Guid EmployeeId)
        {
            var result = await sender.Send(new GetLeavesByEmployeeIdQuery(EmployeeId));
            return Ok(result);

        }

        [HttpPut]
        public async Task<IActionResult> UpdateSalayByEmpId(Guid EmployeeId, LeaveEntity LeaveEntity)
        {
            var result = await sender.Send(new UpdateLeaveCommand(EmployeeId, LeaveEntity));
            if (result == null)
            {
                return BadRequest("Not found the entity to update.");
            }
            return Ok(result);

        }
        [HttpDelete]
        public async Task<IActionResult> DeleteLeaveByEmpId(Guid EmployeeId)
        {
            var result = await sender.Send(new DeleteLeaveByEmpIdCommand(EmployeeId));
            return Ok(result);

        }
    }
}
