
using Management.Core.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Employee.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RefreshTokenController(ISender sender): ControllerBase
    {
        [HttpPost("getaccesstoken")]
        public async Task<IActionResult> GetAccessToken(string Token)
        {
            var result = await sender.Send(new Application.Commands.RefreshToken.RefreshTokenCommand(Token));
            return Ok(result);
        }
    }
}
