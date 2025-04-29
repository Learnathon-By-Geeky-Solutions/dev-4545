using System.Runtime.InteropServices;
using Employee.Application.Commands.Feature;
using Employee.Application.Commands.Project;
using Employee.Application.Queries.Feature;
using Employee.Application.Queries.Project;
using Employee.Application.Queries.Task;
using Employee.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Employee.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FeaturesController(ISender sender , IAuthorizationService authz) : ControllerBase
    {
        private readonly IAuthorizationService _authz = authz;

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllFeatures()
        {
            var result = await sender.Send(new GetAllFeaturesQuery());
            return Ok(result);
        }
        [Authorize(Roles = "Admin,SE")]
        [HttpGet("employees/{EmployeeId:guid}")]
        public async Task<IActionResult> GetFeaturesByEmployeeId(Guid EmployeeId)
        {
            var authResult = await _authz.AuthorizeAsync(User, EmployeeId, "CanModifyOwnEmployee");
            if (!authResult.Succeeded)
                return Forbid();
            var result = await sender.Send(new GetFeatureByEmployeeIdQuery(EmployeeId));
            return Ok(result);
        }

        [Authorize(Roles = "Admin,SE")]
        [HttpGet("{Id:guid}")]
        public async Task<IActionResult> GetFeaturesById(Guid Id)
        {
            var result = await sender.Send(new GetFeatureByIdQuery(Id));
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddFeatureAsync([FromBody] FeatureEntity Feature)
        {
            var result = await sender.Send(new AddFeatureCommand(Feature));
            return Ok(result);
        }

        [HttpPut("{Id:guid}")]
        [Authorize(Roles = "Admin,SE")]
        public async Task<IActionResult> UpdateFeature(Guid Id, FeatureEntity Feature)
        {
            
            var result = await sender.Send(new UpdateFeatureCommand(Id, Feature));
            if (result == null)
            {
                return BadRequest("Entity Not Found.");
            }
            return Ok(result);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteFeature(Guid Id)
        {
            var result = await sender.Send(new DeleteFeatureCommand(Id));
            return Ok(result);
        }
    }
}
