using Employee.Application.Commands.Feature;
using Employee.Application.Commands.Performance;
using Employee.Application.Queries.Feature;
using Employee.Application.Queries.Performance;
using Employee.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Employee.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PerformancesController(ISender sender) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllPerformances()
        {
            var result = await sender.Send(new GetAllPerformancesQuery());
            return Ok(result);
        }
        [HttpGet("performance")]
        public async Task<IActionResult> GetPerformancesById(Guid Id)
        {
            var result = await sender.Send(new GetPerformancesByIdQuery(Id));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddPerformanceAsync([FromBody] PerformanceEntity performance)
        {
            var result = await sender.Send(new AddPerformanceCommand(performance));
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePerformance(Guid Id,PerformanceEntity performance)
        {
            var result = await sender.Send(new UpdatePerformanceCommand(Id, performance));
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePerformance(Guid Id)
        {
            var result = await sender.Send(new DeletePerformanceCommand(Id));
            return Ok(result);
        }
    }
}
