using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TraffiLearn.Application.UseCases.ServiceCenters.Commands.Create;
using TraffiLearn.Application.UseCases.ServiceCenters.Commands.Delete;
using TraffiLearn.Application.UseCases.ServiceCenters.Commands.Update;
using TraffiLearn.Application.UseCases.ServiceCenters.DTO;
using TraffiLearn.Application.UseCases.ServiceCenters.Queries.GetAll;
using TraffiLearn.Application.UseCases.ServiceCenters.Queries.GetById;
using TraffiLearn.Application.UseCases.ServiceCenters.Queries.GetByRegionId;
using TraffiLearn.Infrastructure.Authentication;
using TraffiLearn.WebAPI.Extensions;
using TraffiLearn.WebAPI.Swagger;

namespace TraffiLearn.WebAPI.Controllers
{
    [HasPermission(Permission.AccessData)]
    [Route("api/service-centers")]
    [ApiController]
    public class ServiceCentersController : ControllerBase
    {
        private readonly ISender _sender;

        public ServiceCentersController(ISender sender)
        {
            _sender = sender;
        }

        #region Queries


        /// <summary>
        /// Gets all service centers from the storage.
        /// </summary>
        /// <remarks>
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.
        /// </remarks>
        /// <response code="200">Successfully retrieved all service centers. Returns a list of service centers.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<ServiceCenterResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllServiceCenters()
        {
            var queryResult = await _sender.Send(new GetAllServiceCentersQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <summary>
        /// Gets a service center with a specific ID.
        /// </summary>
        /// <remarks>
        /// **The request must include an ID of a service center to get.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `ServiceCenterId` : Must be a valid GUID representing ID of a service center.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.<br /><br />
        /// </remarks>
        /// <param name="serviceCenterId">**The ID of a service center to be retrieved**</param>
        /// <response code="200">Successfully retrieved the service center with the provided ID. Returns the found service center.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="404">***Not found.*** No service center exists with the provided ID.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpGet("{serviceCenterId:guid}")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ServiceCenterResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetServiceCenterById(
            [FromRoute] Guid serviceCenterId)
        {
            var queryResult = await _sender.Send(
                new GetServiceCenterByIdQuery(serviceCenterId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <summary>
        /// Gets service centers within a certain region.
        /// </summary>
        /// <remarks>
        /// **The request must include an ID of a region.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `RegionId` : Must be a valid GUID representing ID of a region used to find related service centers. Must not be empty.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.<br /><br />
        /// </remarks>
        /// <param name="regionId">**The ID of a region used to find related service centers.**</param>
        /// <response code="200">Successfully retrieved service centers using the provided region ID. Returns the found service centers.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="404">***Not found.*** No region exists with the provided ID.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpGet("from-region/{regionId:guid}")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(IEnumerable<ServiceCenterResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetServiceCentersByRegionId(
            [FromRoute] Guid regionId)
        {
            var queryResult = await _sender.Send(
                new GetServiceCentersByRegionIdQuery(regionId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }


        #endregion

        #region Commands


        /// <summary>
        /// Creates a new service center.
        /// </summary>
        /// <remarks>
        /// ***Body Parameters:***<br /><br />
        /// `RegionId` : Represents a region associated with the service center. Must be a valid GUID. Must not be empty.<br /><br />
        /// `ServiceCenterNumber` : Represents number of the service center. Must not be empty. Must be less than 7 characters long. Must be a number.<br /><br />
        /// `LocationName` : Represents location name in address of the service center (e.g. city, village, etc.). Must not be empty. Must be less than 200 characters long.<br /><br />
        /// `RoadName` : Represents road name in address of the service center (e.g. street or prospect name, etc.). Must not be empty. Must be less than 200 characters long.<br /><br />
        /// `BuildingNumber` : Represents number of building in address of the service center. Must not be empty. Must be less than 25 characters long.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="command">**The create service center command.**</param>
        /// <response code="201">Successfully created a new service center. Returns ID of a newly created service center</response>
        /// <response code="400">***Bad request.*** The provided data is invalid or missing.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** No region exists with the provided region ID.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.ModifyData)]
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateServiceCenter(
            [FromBody] CreateServiceCenterCommand command)
        {
            var commandResult = await _sender.Send(command);

            if (commandResult.IsSuccess)
            {
                return CreatedAtAction(
                    actionName: nameof(GetServiceCenterById),
                    routeValues: new { serviceCenterId = commandResult.Value },
                    value: commandResult.Value);
            }

            return commandResult.ToProblemDetails();
        }

        /// <summary>
        /// Updates an existing service center.
        /// </summary>
        /// <remarks>
        /// If passed new RegionId, it's going to be updated in the region as well.<br /><br />
        /// ***Body Parameters:***<br /><br />
        /// `ServiceCenterId` : Represents a service center by ID to be updated. Must be a valid GUID. Must not be empty.<br /><br />
        /// `RegionId` : Represents a region associated with the service center. Must be a valid GUID. Must not be empty.<br /><br />
        /// `ServiceCenterNumber` : Represents number of the service center. Must not be empty. Must be less than 7 characters long. Must be a number.<br /><br />
        /// `LocationName` : Represents location name in address of the service center (e.g. city, village, etc.). Must not be empty. Must be less than 200 characters long.<br /><br />
        /// `RoadName` : Represents road name in address of the service center (e.g. street or prospect name, etc.). Must not be empty. Must be less than 200 characters long.<br /><br />
        /// `BuildingNumber` : Represents number of building in address of the service center. Must not be empty. Must be less than 25 characters long.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="command">**The update service center command.**</param>
        /// <response code="204">Successfully updated an existing service center.</response>
        /// <response code="400">***Bad request.*** The provided data is invalid or missing.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** Region or Service center with the provided IDs are not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.ModifyData)]
        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateServiceCenter(
            [FromBody] UpdateServiceCenterCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <summary>
        /// Deletes a service center using its ID.
        /// </summary>
        /// <remarks>
        /// **The request must include the ID of a service center.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `ServiceCenterId` : Must be a valid GUID representing ID of a service center.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="serviceCenterId">**The ID of the service center to be deleted.**</param>
        /// <response code="204">Successfully deleted the service center.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** Service center with the provided id is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.ModifyData)]
        [HttpDelete("{serviceCenterId:guid}")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteServiceCenter(
            [FromRoute] Guid serviceCenterId)
        {
            var commandResult = await _sender.Send(new DeleteServiceCenterCommand(serviceCenterId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }


        #endregion
    }
}
