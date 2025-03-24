using Employee.Application.Commands.Feature;
using Employee.Application.Commands.Project;
using Employee.Application.Queries.Feature;
using Employee.Application.Queries.Project;
using Employee.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Employee.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FeaturesController(ISender sender) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllFeatures()
        {
            var result = await sender.Send(new GetAllFeaturesQuery());
            return Ok(result);
        }
        [HttpGet("feature")]
        public async Task<IActionResult> GetFeaturesById(Guid Id)
        {
            var result = await sender.Send(new GetFeatureByIdQuery(Id));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddFeatureAsync([FromBody] FeatureEntity Feature)
        {
            var result = await sender.Send(new AddFeatureCommand(Feature));
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFeature(Guid Id, FeatureEntity Feature)
        {
            var result = await sender.Send(new UpdateFeatureCommand(Id, Feature));
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteFeature(Guid Id)
        {
            var result = await sender.Send(new DeleteFeatureCommand(Id));
            return Ok(result);
        }
    }
}
