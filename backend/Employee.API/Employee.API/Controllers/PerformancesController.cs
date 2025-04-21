﻿using Employee.Application.Commands.Feature;
using Employee.Application.Commands.Performance;
using Employee.Application.Queries.Feature;
using Employee.Application.Queries.Performance;
using Employee.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Employee.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerformancesController(ISender sender) : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllPerformances()
        {
            var result = await sender.Send(new GetAllPerformancesQuery());
            return Ok(result);
        }
        [Authorize(Roles = "Admin,SE")]
        [HttpGet("EmployeeId")]
        public async Task<IActionResult> GetPerformancesByEmployeeId(Guid EmployeeId)
        {
            var result = await sender.Send(new GetPerformancesByIdQuery(EmployeeId));
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddPerformanceAsync([FromBody] PerformanceEntity performance)
        {
            var result = await sender.Send(new AddPerformanceCommand(performance));
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdatePerformance(Guid Id,PerformanceEntity performance)
        {
            var result = await sender.Send(new UpdatePerformanceCommand(Id, performance));
            if (result == null)
            {
                return BadRequest("Not found the entity to update"); 
            }
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeletePerformance(Guid Id)
        {
            var result = await sender.Send(new DeletePerformanceCommand(Id));
            return Ok(result);
        }
    }
}
