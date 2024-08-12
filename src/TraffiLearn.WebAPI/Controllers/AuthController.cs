using MediatR;
using Microsoft.AspNetCore.Mvc;
using TraffiLearn.Application.Commands.Auth.Login;
using TraffiLearn.Application.Commands.Auth.RegisterAdmin;
using TraffiLearn.Application.Commands.Auth.RegisterUser;
using TraffiLearn.Application.Commands.Auth.RemoveAdminAccount;
using TraffiLearn.Infrastructure.Authentication;
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

        #region Commands


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? Ok(commandResult.Value) : commandResult.ToProblemDetails();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? Created() : commandResult.ToProblemDetails();
        }

        [HasPermission(Permission.RegisterAdmins)]
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin(RegisterAdminCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? Created() : commandResult.ToProblemDetails();
        }

        [HasPermission(Permission.RemoveAdmins)]
        [HttpDelete("remove-admin/{adminId:guid}")]
        public async Task<IActionResult> RemoveAdminAccount(Guid adminId)
        {
            var commandResult = await _sender.Send(new RemoveAdminAccountCommand(adminId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }


        #endregion
    }
}
