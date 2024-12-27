using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TraffiLearn.Application.UseCases.Auth.Commands.ConfirmChangeEmail;
using TraffiLearn.Application.UseCases.Auth.Commands.ConfirmEmail;
using TraffiLearn.Application.UseCases.Auth.Commands.Login;
using TraffiLearn.Application.UseCases.Auth.Commands.RecoverPassword;
using TraffiLearn.Application.UseCases.Auth.Commands.RefreshToken;
using TraffiLearn.Application.UseCases.Auth.Commands.RegisterAdmin;
using TraffiLearn.Application.UseCases.Auth.Commands.RegisterUser;
using TraffiLearn.Application.UseCases.Auth.Commands.RemoveAdminAccount;
using TraffiLearn.Application.UseCases.Auth.Commands.ResendConfirmationEmail;
using TraffiLearn.Application.UseCases.Auth.Commands.SendChangeEmailMessage;
using TraffiLearn.Application.UseCases.Auth.Commands.SendRecoverPasswordMessage;
using TraffiLearn.Application.UseCases.Auth.Commands.SignInWithGoogle;
using TraffiLearn.Application.UseCases.Auth.DTO;
using TraffiLearn.Domain.Directories;
using TraffiLearn.Infrastructure.Authentication;
using TraffiLearn.WebAPI.Extensions;
using TraffiLearn.WebAPI.Swagger;

namespace TraffiLearn.WebAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public sealed class AuthController : ControllerBase
    {
        private readonly IDirectoryRepository _directoryRepository;
        private readonly ISender _sender;

        public AuthController(ISender sender, IDirectoryRepository directoryRepository)
        {
            _sender = sender;
            _directoryRepository = directoryRepository;
        }

        #region Commands


        /// <include file='Documentation/AuthControllerDocs.xml' path='doc/members/member[@name="M:Login"]/*'/>
        [HttpPost("login")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(
            [FromBody] LoginCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? Ok(commandResult.Value) : commandResult.ToProblemDetails();
        }

        /// <include file='Documentation/AuthControllerDocs.xml' path='doc/members/member[@name="M:SignInWithGoogle"]/*'/>
        [HttpPost("login/with-google")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SignInWithGoogle(
            [FromBody] SignInWithGoogleCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? Ok(commandResult.Value) : commandResult.ToProblemDetails();
        }

        /// <include file='Documentation/AuthControllerDocs.xml' path='doc/members/member[@name="M:Register"]/*'/>
        [HttpPost("register")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register(
            [FromBody] RegisterUserCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? Created() : commandResult.ToProblemDetails();
        }

        /// <include file='Documentation/AuthControllerDocs.xml' path='doc/members/member[@name="M:ConfirmEmail"]/*'/>
        [HttpPost("confirm-email")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConfirmEmail(
            [FromBody] ConfirmEmailCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <include file='Documentation/AuthControllerDocs.xml' path='doc/members/member[@name="M:ResendConfirmationEmail"]/*'/>
        [HttpPost("resend-confirmation-email")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResendConfirmationEmail(
            [FromBody] ResendConfirmationEmailCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <include file='Documentation/AuthControllerDocs.xml' path='doc/members/member[@name="M:SendChangeEmailMessage"]/*'/>
        [HttpPost("change-email")]
        [HasPermission(Permission.AuthenticatedUser)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendChangeEmailMessage(
            [FromBody] SendChangeEmailMessageCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <include file='Documentation/AuthControllerDocs.xml' path='doc/members/member[@name="M:ConfirmChangeEmail"]/*'/>
        [HttpPost("confirm-change-email")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConfirmChangeEmail(
            [FromBody] ConfirmChangeEmailCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <include file='Documentation/AuthControllerDocs.xml' path='doc/members/member[@name="M:SendRecoverPasswordMessage"]/*'/>
        [HttpPost("recover-password")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendRecoverPasswordMessage(
            [FromBody] SendRecoverPasswordMessageCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <include file='Documentation/AuthControllerDocs.xml' path='doc/members/member[@name="M:RecoverPassword"]/*'/>
        [HttpPost("confirm-password-recovery")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RecoverPassword(
            [FromBody] RecoverPasswordCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <include file='Documentation/AuthControllerDocs.xml' path='doc/members/member[@name="M:RegisterAdmin"]/*'/>
        [HasPermission(Permission.ManageAdmins)]
        [HttpPost("register-admin")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterAdmin(
            [FromBody] RegisterAdminCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? Created() : commandResult.ToProblemDetails();
        }

        /// <include file='Documentation/AuthControllerDocs.xml' path='doc/members/member[@name="M:RemoveAdminAccount"]/*'/>
        [HasPermission(Permission.ManageAdmins)]
        [HttpDelete("remove-admin/{adminId:guid}")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveAdminAccount(
            [FromRoute] Guid adminId)
        {
            var commandResult = await _sender.Send(new RemoveAdminAccountCommand(adminId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <include file='Documentation/AuthControllerDocs.xml' path='doc/members/member[@name="M:Refresh"]/*'/>
        [HttpPost("refresh")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(RefreshTokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Refresh(
            [FromBody] RefreshTokenCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? Ok(commandResult.Value) : commandResult.ToProblemDetails();
        }


        #endregion
    }
}
