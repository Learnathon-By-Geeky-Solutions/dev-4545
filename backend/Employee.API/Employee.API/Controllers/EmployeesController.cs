using Employee.Application.Commands.Employee;
using Employee.Application.Queries.Employee;
using Employee.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Employee.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController(ISender sender) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            
            
            var result = await sender.Send(new GetEmployeeQuery());
            return Ok(result);
        }

        [HttpGet("employee")]
        public async Task<IActionResult> GetEmployeeById(Guid Id)
        {
            var result = await sender.Send(new GetEmployeeByIdQuery(Id));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployeeAsync([FromBody] EmployeeEntity employee)
        {
            var result = await sender.Send(new AddEmployeeCommand(employee));
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployee(Guid Id, EmployeeEntity employee)
        {
            var result = await sender.Send(new UpdateEmployeeCommand(Id,employee));
            if (result == null)
            {
                return BadRequest("Not found the entity to update");
            }
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEmployee(Guid Id)
        {
            var result = await sender.Send(new DeleteEmployeeCommand(Id));
            return Ok(result);
        }
    }
}
