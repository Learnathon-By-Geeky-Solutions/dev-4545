using Employee.Application.Commands.Employee;
using Employee.Application.Queries.Employee;
using Employee.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Employee.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]                          // must at least be authenticated
    public class EmployeesController(ISender sender, IAuthorizationService authz) : ControllerBase
    {
        private readonly ISender _sender = sender;
        private readonly IAuthorizationService _authz = authz;

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
            => Ok(await _sender.Send(new GetEmployeeQuery()));

        [Authorize(Roles = "Admin,SE")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetEmployeeById(Guid id)
        {
            var authResult = await _authz.AuthorizeAsync(User, id, "CanModifyOwnEmployee");
            if (!authResult.Succeeded)
                return Forbid();
            var result = await _sender.Send(new GetEmployeeByIdQuery(id));
            return Ok(result);
        }


        // --- Admin only: add
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeEntity e)
            => Ok(await _sender.Send(new AddEmployeeCommand(e)));

        // --- Admin or “own” employee: update
        [Authorize(Roles = "Admin,SE")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] EmployeeEntity e)
        {
            // policy check: will succeed for Admin OR if id == my employeeId claim
            var authResult = await _authz.AuthorizeAsync(User, id, "CanModifyOwnEmployee");
            if (!authResult.Succeeded)
                return Forbid();

            var updated = await _sender.Send(new UpdateEmployeeCommand(id, e));
            return updated is null
                ? NotFound()
                : Ok(updated);
        }

        // --- Admin or “own” employee: delete
        [Authorize(Roles = "Admin,SE")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var authResult = await _authz.AuthorizeAsync(User, id, "CanModifyOwnEmployee");
            if (!authResult.Succeeded)
                return Forbid();

            await _sender.Send(new DeleteEmployeeCommand(id));
            return NoContent();
        }
    }
}