using System;
using System.Threading.Tasks;
using Employee.Application.Commands.Salary;
using Employee.Application.Queries.Salary;
using Employee.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Employee.API.Controllers
{
    [Authorize] // Require authenticated user for all actions by default
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryController(ISender sender, IAuthorizationService authz) : ControllerBase
    {
        private readonly ISender _sender = sender;
        private readonly IAuthorizationService _authz = authz;

        // Only Admin can insert a salary
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> InsertSalary([FromBody] SalaryEntity salary)
        {
            var result = await _sender.Send(new AddSalaryCommand(salary));
            return Ok(result);
        }

        // Admin and SE can view all salaries
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSalaries()
        {
            var result = await _sender.Send(new GetSalaryQuery());
            return Ok(result);
        }

        // Admin and SE can view salary by employee ID
        [HttpGet("{EmployeeId:guid}")]
        [Authorize(Roles = "Admin,SE")]
        public async Task<IActionResult> GetSalaryByEmpId(Guid EmployeeId)
        {
            //var authResult = await _authz.AuthorizeAsync(User, EmployeeId, "CanModifyOwnEmployee");
            //if (!authResult.Succeeded)
            //    return Forbid();
            var result = await _sender.Send(new GetSalariesByEmployeeIdQuery(EmployeeId));
            if (result == null)
            {
                return BadRequest("There is no salary updated for the employee."); 
            }
            return Ok(result);
        }

        // Only Admin can update salary by employee ID
        [HttpPut("{employeeId:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSalaryByEmpId(Guid employeeId, [FromBody] SalaryEntity salaryEntity)
        {
            var result = await _sender.Send(new UpdateSalaryCommand(employeeId, salaryEntity));
            return result != null ? Ok(result) : BadRequest("Entity not found to update.");
        }

        // Only Admin can delete salary by employee ID
        [HttpDelete("{employeeId:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSalaryByEmpId(Guid employeeId)
        {
            var result = await _sender.Send(new DeleteSalaryByEmpIdCommand(employeeId));
            return Ok(result);
        }
    }
}
