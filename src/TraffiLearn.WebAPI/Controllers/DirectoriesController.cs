using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TraffiLearn.Application.Directories.Commands.Create;
using TraffiLearn.Application.Directories.Commands.Update;
using TraffiLearn.Application.Directories.Commands.Delete;
using TraffiLearn.Application.Directories.DTO;
using TraffiLearn.Application.Directories.Queries.GetAll;
using TraffiLearn.Application.Directories.Queries.GetById;
using TraffiLearn.Infrastructure.Authentication;
using TraffiLearn.WebAPI.Extensions;
using TraffiLearn.WebAPI.Swagger;

namespace TraffiLearn.WebAPI.Controllers
{
    [HasPermission(Permission.AccessData)]
    [Route("api/directories")]
    [ApiController]
    public class DirectoriesController : ControllerBase
    {
        private readonly ISender _sender;

        public DirectoriesController(ISender sender)
        {
            _sender = sender;
        }

        #region Queries


        /// <summary>
        /// Gets all directories from the storage.
        /// </summary>
        /// <remarks>
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.
        /// </remarks>
        /// <response code="200">Successfully retrieved all directories. Returns a list of directories.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<DirectoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllDirectories()
        {
            var queryResult = await _sender.Send(new GetAllDirectoriesQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <summary>
        /// Gets a directory with a specific ID.
        /// </summary>
        /// <remarks>
        /// **The request must include an ID of a directory to get.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `DirectoryId` : Must be a valid GUID representing ID of a directory.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.<br /><br />
        /// </remarks>
        /// <param name="directoryId">**The ID of a directory to be retrieved**</param>
        /// <response code="200">Successfully retrieved the directory with the provided ID. Returns the found directory.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="404">***Not found.*** No directory exists with the provided ID.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpGet("{directoryId:guid}")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(DirectoryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDirectoryById(
            [FromRoute] Guid directoryId)
        {
            var queryResult = await _sender.Send(new GetDirectoryByIdQuery(directoryId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }
        
        
        #endregion

        #region Commands


        /// <summary>
        /// Creates a new directory.
        /// </summary>
        /// <remarks>
        /// If succesfully created a new directory, this endpoint returns ID of the newly created directory.<br /><br />
        /// <remarks>
        /// ***Parameters:***<br /><br />
        /// `Name`: Name of the directory to create. Must not be empty or whitespace. Maximum length: 200. <br /><br />
        /// `Sections`: List of sections to create in the directory. Must not be empty. Must contain at least one section. Maximum count: 100. <br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="command">**The create directory command.**</param>
        /// <response code="201">Successfully created a new directory. Returns ID of a newly created directory.</response>
        /// <response code="400">***Bad request.*** The provided data is invalid or missing.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpPost]
        [HasPermission(Permission.ModifyData)]
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

            return commandResult.ToProblemDetails();
        }
        
        /// <summary>
        /// Updates an existing directory.
        /// </summary>
        /// <remarks>
        /// ***Body Parameters:***<br /><br />
        /// `Name`: Name of the directory to create. Must not be empty or whitespace. Maximum length: 200. <br /><br />
        /// `Sections`: List of sections to create in the directory. Must not be empty. Must contain at least one section. Maximum count: 100. <br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="command">The update directory command.</param>
        /// <response code="204">Successfully updated an existing directory.</response>
        /// <response code="400">***Bad request.*** The provided data is invalid or missing.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** Directory with the ID is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.ModifyData)]
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateDirectory(
            [FromBody] UpdateDirectoryCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }
        
        /// <summary>
        /// Deletes a directory using its ID.
        /// </summary>
        /// <remarks>
        /// **The request must include the ID of the directory.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `DirectoryId` : Must be a valid GUID representing ID of the directory.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="directoryId">**The ID of the directory to be deleted.**</param>  
        /// <response code="204">Successfully deleted the directory.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** Directory with the provided id is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.ModifyData)]
        [HttpDelete("{directoryId:guid}")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteDirectory(
            [FromRoute] Guid directoryId)
        {
            var commandResult = await _sender.Send(new DeleteDirectoryCommand(directoryId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }


        #endregion
    }
}
