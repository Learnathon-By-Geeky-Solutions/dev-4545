using Employee.Application.Commands.Salary;
using Employee.Application.Queries.Salary;
using Employee.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Employee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> InsertSalary(SalaryEntity salary) {
            var result =await sender.Send(new AddSalaryCommand(salary));
            return Ok(result);

        }
        [HttpGet]
        public async Task<IActionResult> GetSalaries()
        {
            var result = await sender.Send(new GetSalaryQuery());
            return Ok(result);

        }
        [HttpGet("GetSalaryByEmployeeId")]
        public async Task<IActionResult> GetSalaryByEmpId(Guid EmployeeId)
        {
            var result= await sender.Send(new GetSalariesByEmployeeIdQuery(EmployeeId));
            return Ok(result);

        }

        [HttpPut]
        public async Task<IActionResult> UpdateSalayByEmpId(Guid EmployeeId, SalaryEntity salaryEntity)
        {
            var result = await sender.Send(new UpdateSalaryCommand(EmployeeId,salaryEntity));
            if (result == null) {
                return BadRequest("Entity Not Found to Update.");
            }
            return Ok(result);  

        }
        [HttpDelete]
        public async Task<IActionResult> DeleteSalaryByEmpId(Guid EmployeeId)
        {
            var result=await sender.Send( new DeleteSalaryByEmpIdCommand(EmployeeId));
            return Ok(result);

        }
    }
}
