using Management.Core.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Employee.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(ISender sender) : ControllerBase
    {
        [HttpPost("")]
        public async Task<IActionResult> Authentication([FromBody] AuthenticationRequest employee)
        {
            var result = await sender.Send(new Application.Commands.Login.LoginCommand(employee));
            return Ok(result);
        }
    }
}
