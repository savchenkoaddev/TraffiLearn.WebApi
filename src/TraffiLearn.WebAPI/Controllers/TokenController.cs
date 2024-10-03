using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TraffiLearn.Application.Auth.Commands.RefreshToken;
using TraffiLearn.Application.Auth.DTO;
using TraffiLearn.Infrastructure.Authentication;
using TraffiLearn.WebAPI.Extensions;
using TraffiLearn.WebAPI.Swagger;

namespace TraffiLearn.WebAPI.Controllers
{
    [HasPermission(Permission.AccessData)]
    [Route("api/token")]
    [ApiController]
    public class TokenController : Controller
    {
        private readonly ISender _sender;

        public TokenController(ISender sender)
        {
            _sender = sender;
        }

        #region Commands


        /// <summary>
        /// Refreshes the access token using the refresh token
        /// </summary>
        /// <remarks>
        /// **The request must include a valid access and refresh tokens which meet the required criteria.**<br /><br /><br />
        /// **After refreshing the tokens, the old refresh token will be replaced with the new one. The old token won't be valid**<br /><br /><br />
        /// ***Body parameters:***<br /><br />
        /// `AccessToken` : Must be a valid access token.<br /><br />
        /// `RefreshToken` : Must be a valid access token.<br /><br /><br />
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
