using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TraffiLearn.Application.Regions.Commands.Create;
using TraffiLearn.Application.Regions.Commands.Update;
using TraffiLearn.Application.Regions.DTO;
using TraffiLearn.Application.Regions.Queries.GetAll;
using TraffiLearn.Application.Regions.Queries.GetById;
using TraffiLearn.Infrastructure.Authentication;
using TraffiLearn.WebAPI.Extensions;
using TraffiLearn.WebAPI.Swagger;

namespace TraffiLearn.WebAPI.Controllers
{
    [HasPermission(Permission.AccessData)]
    [Route("api/regions")]
    [ApiController]
    public sealed class RegionsController : ControllerBase
    {
        private readonly ISender _sender;

        public RegionsController(ISender sender)
        {
            _sender = sender;
        }

        #region Queries


        /// <summary>
        /// Gets all regions from the storage.
        /// </summary>
        /// <remarks>
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.
        /// </remarks>
        /// <response code="200">Successfully retrieved all regions. Returns a list of regions.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<RegionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllRegions()
        {
            var queryResult = await _sender.Send(new GetAllRegionsQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <summary>
        /// Gets a region with a specific ID.
        /// </summary>
        /// <remarks>
        /// **The request must include an ID of a region to get.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `RegionId` : Must be a valid GUID representing ID of a region.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.<br /><br />
        /// </remarks>
        /// <param name="regionId">**The ID of a region to be retrieved**</param>
        /// <response code="200">Successfully retrieved the region with the provided ID. Returns the found region.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="404">***Not found.*** No region exists with the provided ID.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpGet("{regionId:guid}")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(RegionResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRegionById(
            [FromRoute] Guid regionId)
        {
            var queryResult = await _sender.Send(new GetRegionByIdQuery(regionId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }


        #endregion

        #region Commands


        /// <summary>
        /// Creates a new region.
        /// </summary>
        /// <remarks>
        /// ***Parameters:***<br /><br />
        /// `RegionName` : Represents name of a region. Must not be empty. Must be less than 100 characters long.<br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="command">**The create region command.**</param>
        /// <response code="201">Successfully created a new region. Returns ID of a newly created region</response>
        /// <response code="400">***Bad request.*** The provided data is invalid or missing.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.ModifyData)]
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateRegion(CreateRegionCommand command)
        {
            var commandResult = await _sender.Send(command);

            if (commandResult.IsSuccess)
            {
                return CreatedAtAction(
                    actionName: nameof(GetRegionById),
                    routeValues: new { regionId = commandResult.Value },
                    value: commandResult.Value);
            }

            return commandResult.ToProblemDetails();
        }

        /// <summary>
        /// Updates an existing region.
        /// </summary>
        /// <remarks>
        /// ***Body Parameters:***<br /><br />
        /// `RegionId` : ID of the region to be updated. Must be a valid GUID.<br /><br />
        /// `RegionName` : Represents a new name of a region. Must not be empty. Must be less than 100 characters long.<br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="command">**The update region command.**</param>
        /// <response code="204">Successfully updated an existing region.</response>
        /// <response code="400">***Bad request.*** The provided data is invalid or missing.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** Region with the ID is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.ModifyData)]
        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateRegion(UpdateRegionCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }


        #endregion
    }
}
