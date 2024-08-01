using MediatR;
using Microsoft.AspNetCore.Mvc;
using TraffiLearn.Application.Commands.Auth.Login;
using TraffiLearn.Application.Commands.Auth.RegisterUser;
using TraffiLearn.WebAPI.Extensions;

namespace TraffiLearn.WebAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;

        public AuthController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterUserCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? Created() : commandResult.ToProblemDetails();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? Ok(commandResult.Value) : commandResult.ToProblemDetails();
        }
    }
}
