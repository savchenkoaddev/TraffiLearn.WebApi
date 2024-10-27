using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TraffiLearn.Application.Auth.Commands.ConfirmChangeEmail;
using TraffiLearn.Application.Auth.Commands.ConfirmEmail;
using TraffiLearn.Application.Auth.Commands.Login;
using TraffiLearn.Application.Auth.Commands.RecoverPassword;
using TraffiLearn.Application.Auth.Commands.RefreshToken;
using TraffiLearn.Application.Auth.Commands.RegisterAdmin;
using TraffiLearn.Application.Auth.Commands.RegisterUser;
using TraffiLearn.Application.Auth.Commands.RemoveAdminAccount;
using TraffiLearn.Application.Auth.Commands.SendChangeEmailMessage;
using TraffiLearn.Application.Auth.Commands.SendRecoverPasswordMessage;
using TraffiLearn.Application.Auth.Commands.SignInWithGoogle;
using TraffiLearn.Application.Auth.DTO;
using TraffiLearn.Domain.Aggregates.Directories;
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


        /// <summary>
        /// Authenticates a user using their email and password.
        /// </summary>
        /// <remarks>
        /// **The request must include a valid email and a password that meets the required criteria.**<br /><br /><br />
        /// ***Body parameters:***<br /><br />
        /// `Email` : Must be in a valid email format (e.g., example@example.com).<br /><br />
        /// `Password` : Must be between 8 and 30 characters long, include at least one uppercase letter, one lowercase letter, one number, and one special character.<br /><br /><br />
        /// The request body must be in **JSON** format.<br /><br />
        /// **Example request:**<br />
        /// ```json
        /// {
        ///     "email": "example@example.com",
        ///     "password": "P@ssw0rd!"
        /// }
        /// ```
        ///
        /// **Example response (200 OK)**:
        /// ```json
        /// {
        ///     "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c"
        ///     "refreshToken": "s1x/m6Xc3H+bWdBVYViBKvNverG2DMtNgt7Ac0nl/fEeTcEfwT5uzCIzaME6H4cy2/r66+Lm7E5Wcyig+Tv8xA=="
        /// }
        /// ```
        /// 
        /// **Access token** is used to authenticate the user for a specific period of time. **Access token** expires in a short time. For example in **20 minutes**. <br /><br />
        /// **Refresh token** is used to obtain a new access token when the current one expires. **Refresh token** expires in a long time. For example in **7 days**. <br /><br />
        /// Every time the user logs in, **refresh token expiration time** is extended (e.g. for 7 days) from current time. <br /><br />
        /// </remarks>
        /// <param name="command">**The login command containing email and password.**</param>
        /// <response code="200">Successfully authenticated. Returns an access and refresh token.</response>
        /// <response code="400">***Bad request.*** Either the email or password is in an incorrect format, or the provided credentials are invalid.</response>
        /// <response code="404">***Not found.*** No user exists with the provided credentials.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
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

        /// <summary>
        /// Authenticates a user through Google Auth.
        /// </summary>
        /// <remarks>
        /// **The request must include a Google ID token and a first time sign in password if the user haven't been registered yet.**<br /><br /><br />
        /// ***Body parameters:***<br /><br />
        /// `GoogleIdToken` : Must be a valid Google ID token. This token is received, when a user authenticates through the Google page.<br /><br />
        /// `FirstTimeSignInPassword` : Must be between 8 and 30 characters long, include at least one uppercase letter, one lowercase letter, one number, and one special character. If the user haven't been registered yet, this is is required, otherwise you get 400 Bad Request error.<br /><br /><br />
        /// The request body must be in **JSON** format.<br /><br />
        /// **Example response (200 OK)**:
        /// ```json
        /// {
        ///     "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c"
        ///     "refreshToken": "s1x/m6Xc3H+bWdBVYViBKvNverG2DMtNgt7Ac0nl/fEeTcEfwT5uzCIzaME6H4cy2/r66+Lm7E5Wcyig+Tv8xA=="
        /// }
        /// ```
        /// 
        /// **Access token** is used to authenticate the user for a specific period of time. **Access token** expires in a short time. For example in **20 minutes**. <br /><br />
        /// **Refresh token** is used to obtain a new access token when the current one expires. **Refresh token** expires in a long time. For example in **7 days**. <br /><br />
        /// Every time the user logs in, **refresh token expiration time** is extended (e.g. for 7 days) from current time. <br /><br />
        /// </remarks>
        /// <param name="command">**The sign in with google command containing email and password.**</param>
        /// <response code="200">Successfully authenticated. Returns an access and refresh token.</response>
        /// <response code="400">***Bad request.*** Either the user is not registered and the first time login password is not provided, or the password is in an incorrect format.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
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

        /// <summary>
        /// Registers a new user account.
        /// </summary>
        /// <remarks>
        /// **The request must include a valid username, email, and password that meets the required criteria.**<br /><br /><br />
        /// ***Body parameters:***<br /><br />
        /// `Username` : Must be a valid string. Must be unique.<br /><br />
        /// `Email` : Must be in a valid email format (e.g., example@example.com). Must be unique.<br /><br />
        /// `Password` : Must be between 8 and 30 characters long, include at least one uppercase letter, one lowercase letter, one number, and one special character.<br /><br /><br />
        /// The request body must be in **JSON** format.<br /><br />
        /// **Example request:**<br />
        /// ```json
        /// {
        ///     "username": "user1",
        ///     "email": "user1@example.com",
        ///     "password": "P@ssw0rd!"
        /// }
        /// ```
        /// </remarks>
        /// <param name="command">**The register user command containing username, email, and password.**</param>
        /// <response code="201">Successfully registered. The user account has been created.</response>
        /// <response code="400">***Bad request.*** The provided data is invalid or missing, or a user with the same username or email already exists.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
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

        /// <summary>
        /// Confirms an email of a user
        /// </summary>
        /// <remarks>
        /// **The request must include a valid user ID and token.**<br /><br /><br />
        /// ***Body parameters:***<br /><br />
        /// `UserId` : Must be a valid GUID representing the user whose email is being confirmed.<br /><br />
        /// `Token` : Represents a token generated by the system.<br /><br /><br />
        /// **Authentication is not required.**
        /// </remarks>
        /// <param name="command">**The confirm email message command.**</param>
        /// <response code="204">Successfully confirmed the email.</response>
        /// <response code="400">***Bad request.*** The provided data is invalid or missing, or the token is invalid or expired.</response>
        /// <response code="404">***Not Found.*** The user with the provided ID is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
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

        /// <summary>
        /// Sends a change email message to a user's emailbox
        /// </summary>
        /// <remarks>
        /// **The request must include a new valid email.**<br /><br /><br />
        /// ***Body parameters:***<br /><br />
        /// `NewEmail` : Must be in a valid email format (e.g., example@example.com). Must be unique.<br /><br /><br />
        /// **Authentication Required.**<br /><br />
        /// The user must be authenticated using a JWT token. 
        /// </remarks>
        /// <param name="command">**The send change email message command.**</param>
        /// <response code="204">Successfully sent the change email message.</response>
        /// <response code="400">***Bad request.*** The provided email is invalid, missing or already taken.</response>
        /// <response code="401">***Unauthorized***. The user is not authenticated.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.AccessData)]
        [HttpPost("change-email")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendChangeEmailMessage(
            [FromBody] SendChangeEmailMessageCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <summary>
        /// Confirms a change of an email for a user
        /// </summary>
        /// <remarks>
        /// **The request must include a valid user ID, token and new email.**<br /><br /><br />
        /// ***Query parameters:***<br /><br />
        /// `UserId` : Must be a valid GUID representing the user whose email is being confirmed.<br /><br />
        /// `Token` : Represents a token generated by the system.<br /><br />
        /// `NewEmail` : Must be in a valid email format (e.g., example@example.com).<br /><br /><br />
        /// **Authentication is not required.**
        /// </remarks>
        /// <param name="command">**The confirm change email command.**</param>
        /// <response code="204">Successfully confirmed the change email request.</response>
        /// <response code="400">***Bad request.*** The provided data is invalid or missing, or the token is invalid or expired.</response>
        /// <response code="404">***Not Found.*** The user with the provided ID is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
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

        /// <summary>
        /// Sends email with a recover password link.
        /// </summary>
        /// <remarks>
        /// **The request must include a valid email.**<br /><br /><br />
        /// **The request sends an email to the user's email address with a link to reset the password.**<br /><br /><br />
        /// ***Body parameters:***<br /><br />
        /// `Email` : Must be in a valid email format (e.g., example@example.com).<br /><br /><br />
        /// </remarks>
        /// <param name="command">**The recover password command.**</param>
        /// <response code="204">Successfully sent the recover password message.</response>
        /// <response code="400">***Bad request.*** The provided email is invalid or missing.</response>
        /// <response code="404">***Not found.*** No user exists with the provided email.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
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

        /// <summary>
        /// Resets the user's password.
        /// </summary>
        /// <remarks>
        /// **The request must include a valid user ID, token, and a new password that meets the required criteria.**<br /><br /><br />
        /// ***Body parameters:***<br /><br />
        /// `UserId` : Must be a valid GUID representing the user whose password is being reset.<br /><br />
        /// `Token` : Represents a token generated by the system.<br /><br />
        /// `NewPassword` : Must be between 8 and 30 characters long, include at least one uppercase letter, one lowercase letter, one number, and one special character.<br /><br /><br />
        /// `RepeatPassword` : Must be the same as the new password.<br /><br /><br />
        /// </remarks>
        /// <response code="204">Successfully reset user's password</response>
        /// <response code="400">***Bad request***. The provided data is invalid or missing, or the token is invalid or expired.</response>
        /// <response code="404">***Not found***. The user with the provided ID is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
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

        /// <summary>
        /// Registers a new admin account.
        /// </summary>
        /// <remarks>
        /// **The request must include a valid username, email, and password that meets the required criteria.**<br /><br /><br />
        /// ***Body parameters:***<br /><br />
        /// `Username` : Must be a valid string. Must be unique.<br /><br />
        /// `Email` : Must be in a valid email format (e.g., example@example.com). Must be unique.<br /><br />
        /// `Password` : Must be between 8 and 30 characters long, include at least one uppercase letter, one lowercase letter, one number, and one special character.<br /><br /><br />
        /// The request body must be in **JSON** format.<br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` role can perform this action.<br /><br />
        /// **Example request:**<br />
        /// ```json
        /// {
        ///     "username": "admin1",
        ///     "email": "admin1@example.com",
        ///     "password": "AdminP@ssw0rd!"
        /// }
        /// ```
        /// </remarks>
        /// <param name="command">**The register admin command containing username, email, and password.**</param>
        /// <response code="201">Successfully registered. The admin account has been created.</response>
        /// <response code="400">***Bad request***. The provided data is invalid or missing, or a user with the same username or email already exists.</response>
        /// <response code="401">***Unauthorized***. The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="500">***Internal Server Error***. An unexpected error occurred during the process.</response>
        [HasPermission(Permission.RegisterAdmins)]
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

        /// <summary>
        /// Removes an existing admin account.
        /// </summary>
        /// <remarks>
        /// **The request must include the ID of the admin account to be removed.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `AdminId` : Must be a valid GUID representing the admin account to be removed.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="adminId">**The admin account ID to be removed.**</param>
        /// <response code="204">Successfully removed the admin account.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden.*** The user is not authorized to perform this action.</response>
        /// <response code="404">***Not Found.*** The admin with the provided ID is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.RemoveAdmins)]
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

        /// <summary>
        /// Refreshes the access token using the refresh token
        /// </summary>
        /// <remarks>
        /// **The request must include a valid access and refresh tokens which meet the required criteria.**<br /><br /><br />
        /// **After refreshing the tokens, the old refresh token will be replaced with the new one. The old token won't be valid**<br /><br /><br />
        /// ***Body parameters:***<br /><br />
        /// `AccessToken` : Must be a valid access token.<br /><br />
        /// `RefreshToken` : Must be a valid refresh token.<br /><br /><br />
        /// </remarks>
        /// <param name="command">The refresh token command.</param>
        /// <response code="200">Successfully generated new access and refresh tokens.</response>
        /// <response code="400">***Bad request.*** The provided data is invalid or missing.</response>
        /// <response code="404">***Not found.*** No user exists with the provided credentials.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
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
