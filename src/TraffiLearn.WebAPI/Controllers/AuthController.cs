using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TraffiLearn.Application.Auth.Commands.ConfirmEmail;
using TraffiLearn.Application.Auth.Commands.Login;
using TraffiLearn.Application.Auth.Commands.RefreshToken;
using TraffiLearn.Application.Auth.Commands.RegisterAdmin;
using TraffiLearn.Application.Auth.Commands.RegisterUser;
using TraffiLearn.Application.Auth.Commands.RemoveAdminAccount;
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
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(
            [FromBody] LoginCommand command)
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
        /// ***Query parameters:***<br /><br />
        /// `UserId` : Must be a valid GUID representing the user whose email is being confirmed.<br /><br />
        /// `Token` : Represents a token generated by the system.<br /><br /><br />
        /// **Authentication is not required.**
        /// </remarks>
        /// <param name="userId">**The ID of the user to be confirmed.**</param>
        /// <param name="token">**The generated token used to confirm the email.**</param>
        /// <response code="204">Successfully confirmed the email.</response>
        /// <response code="400">***Bad request.*** The provided data is invalid or missing, or the token is invalid or expired.</response>
        /// <response code="404">***Not Found.*** The user with the provided ID is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpGet("confirm-email")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConfirmEmail(
            [FromQuery] Guid userId,
            [FromQuery] string token)
        {
            var commandResult = await _sender.Send(new ConfirmEmailCommand(
                UserId: userId,
                Token: token));

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
        [HttpPost]
        [Route("refresh")]
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
