using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Net.Mime;
using TraffiLearn.Application.UseCases.Directories.Commands.Create;
using TraffiLearn.Application.UseCases.Directories.Commands.Delete;
using TraffiLearn.Application.UseCases.Directories.Commands.Update;
using TraffiLearn.Application.UseCases.Directories.DTO;
using TraffiLearn.Application.UseCases.Directories.Queries.GetAll;
using TraffiLearn.Application.UseCases.Directories.Queries.GetById;
using TraffiLearn.Infrastructure.Authentication;
using TraffiLearn.Infrastructure.Extensions.DI;
using TraffiLearn.WebAPI.Extensions;
using TraffiLearn.WebAPI.Factories;
using TraffiLearn.WebAPI.Swagger;

namespace TraffiLearn.WebAPI.Controllers
{
    [HasPermission(Permission.AuthenticatedUser)]
    [EnableRateLimiting(RateLimitingExtensions.DefaultPolicyName)]
    [Route("api/directories")]
    [ApiController]
    public class DirectoriesController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly ProblemDetailsFactory _problemDetailsFactory;

        public DirectoriesController(
            ISender sender,
            ProblemDetailsFactory problemDetailsFactory)
        {
            _sender = sender;
            _problemDetailsFactory = problemDetailsFactory;
        }

        #region Queries


        /// <include file='Documentation/DirectoriesControllerDocs.xml' path='doc/members/member[@name="M:GetAllDirectories"]/*'/>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<DirectoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllDirectories()
        {
            var queryResult = await _sender.Send(new GetAllDirectoriesQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : _problemDetailsFactory.GetProblemDetails(queryResult);
        }

        /// <include file='Documentation/DirectoriesControllerDocs.xml' path='doc/members/member[@name="M:GetDirectoryById"]/*'/>
        [HttpGet("{directoryId:guid}")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(DirectoryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDirectoryById(
            [FromRoute] Guid directoryId)
        {
            var queryResult = await _sender.Send(new GetDirectoryByIdQuery(directoryId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : _problemDetailsFactory.GetProblemDetails(queryResult);
        }


        #endregion

        #region Commands


        /// <include file='Documentation/DirectoriesControllerDocs.xml' path='doc/members/member[@name="M:CreateDirectory"]/*'/>
        [HttpPost]
        [HasPermission(Permission.ModifyApplicationData)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateDirectory(
            [FromBody] CreateDirectoryCommand command)
        {
            var commandResult = await _sender.Send(command);

            if (commandResult.IsSuccess)
            {
                return CreatedAtAction(
                    actionName: nameof(GetDirectoryById),
                    routeValues: new { directoryId = commandResult.Value },
                    value: commandResult.Value);
            }

            return _problemDetailsFactory.GetProblemDetails(commandResult);
        }

        /// <include file='Documentation/DirectoriesControllerDocs.xml' path='doc/members/member[@name="M:UpdateDirectory"]/*'/>
        [HttpPut]
        [HasPermission(Permission.ModifyApplicationData)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateDirectory(
            [FromBody] UpdateDirectoryCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? NoContent() : _problemDetailsFactory.GetProblemDetails(commandResult);
        }

        /// <include file='Documentation/DirectoriesControllerDocs.xml' path='doc/members/member[@name="M:DeleteDirectory"]/*'/>
        [HttpDelete("{directoryId:guid}")]
        [HasPermission(Permission.ModifyApplicationData)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteDirectory(
            [FromRoute] Guid directoryId)
        {
            var commandResult = await _sender.Send(new DeleteDirectoryCommand(directoryId));

            return commandResult.IsSuccess ? NoContent() : _problemDetailsFactory.GetProblemDetails(commandResult);
        }


        #endregion
    }
}
