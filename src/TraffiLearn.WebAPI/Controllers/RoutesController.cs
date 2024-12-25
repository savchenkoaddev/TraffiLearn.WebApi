using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TraffiLearn.Application.UseCases.Routes.Commands.Delete;
using TraffiLearn.Application.UseCases.Routes.DTO;
using TraffiLearn.Application.UseCases.Routes.Queries.GetById;
using TraffiLearn.Application.UseCases.Routes.Queries.GetByServiceCenterId;
using TraffiLearn.Infrastructure.Authentication;
using TraffiLearn.WebAPI.CommandWrappers.CreateRoute;
using TraffiLearn.WebAPI.CommandWrappers.UpdateRoute;
using TraffiLearn.WebAPI.Extensions;
using TraffiLearn.WebAPI.Swagger;
using static System.Net.Mime.MediaTypeNames;

namespace TraffiLearn.WebAPI.Controllers
{
    [HasPermission(Permission.AuthenticatedUser)]
    [ApiController]
    [Route("api/routes")]
    public class RoutesController : ControllerBase
    {
        private readonly ISender _sender;

        public RoutesController(ISender sender)
        {
            _sender = sender;
        }

        #region Queries


        /// <summary>
        /// Gets a route with a specific ID.
        /// </summary>
        /// <remarks>
        /// **The request must include an ID of a route to get.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `RouteId` : Must be a valid GUID representing ID of a route.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.<br /><br />
        /// </remarks>
        /// <param name="routeId">**The ID of a route to be retrieved**</param>
        /// <response code="200">Successfully retrieved the route with the provided ID. Returns the found route.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="404">***Not found.*** No route exists with the provided ID.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpGet("{routeId:guid}")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(RouteResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRouteById(
            [FromRoute] Guid routeId)
        {
            var queryResult = await _sender.Send(new GetRouteByIdQuery(routeId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <summary>
        /// Gets routes within a certain service center.
        /// </summary>
        /// <remarks>
        /// **The request must include an ID of a service center.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `ServiceCenterId` : Must be a valid GUID representing ID of a service center used to find related routes. Must not be empty.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.<br /><br />
        /// </remarks>
        /// <param name="serviceCenterId">**The ID of a service center used to find related routes.**</param>
        /// <response code="200">Successfully retrieved routes using the provided service center ID. Returns the found routes.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="404">***Not found.*** No service center exists with the provided ID.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpGet("from-service-center/{serviceCenterId:guid}")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(IEnumerable<RouteResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRoutesByServiceCenterId(
            [FromRoute] Guid serviceCenterId)
        {
            var queryResult = await _sender.Send(
                new GetRoutesByServiceCenterIdQuery(serviceCenterId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }


        #endregion

        #region Commands


        /// <summary>
        /// Creates a new route.
        /// </summary>
        /// <remarks>
        /// **Image is required.** After the successful execution, the route is going to be assigned with image URI, where the provided image can be accessed.<br /><br />
        /// **If succesfully created a new route**, the route will be associated with the service center (using its ID). It means the route is going to contain the service center and the service center is going to contain the route as well.<br /><br /><br />
        /// ***Parameters:***<br /><br />
        /// `Image` : Must be a valid image (possible extensions: ".jpg", ".jpeg", ".png", ".gif", ".bmp"). The size must be less than 500 Kb. Required field.<br /><br />
        /// `Request` : Route represented as a JSON object.<br /><br /><br />
        /// **Request parameters:**<br /><br />
        /// `ServiceCenterId`: Represents ID of a service center to be associated with this route. Must be a valid GUID.<br /><br />
        /// `RouteNumber` : Represents number of the route. Must be an integer with the minimum value of 1.<br /><br />
        /// `Description` : Represents description of the route. Must be a string with the maximum length of 2000 characters. Not required field.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="command">**The create route command.**</param>
        /// <response code="201">Successfully created a new route. Returns ID of a newly created route</response>
        /// <response code="400">***Bad request.*** The provided data is invalid or missing.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** Service center with the provided service center ID is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpPost]
        [HasPermission(Permission.ModifyApplicationData)]
        [Consumes(Multipart.FormData)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateRoute(
            [FromForm] CreateRouteCommandWrapper command)
        {
            var commandResult = await _sender.Send(command.ToCommand());

            if (commandResult.IsSuccess)
            {
                return CreatedAtAction(
                    actionName: nameof(GetRouteById),
                    routeValues: new { routeId = commandResult.Value },
                    value: commandResult.Value);
            }

            return commandResult.ToProblemDetails();
        }

        /// <summary>
        /// Updates an existing route.
        /// </summary>
        /// <remarks>
        /// **If provided new image**, the old image gets deleted, the new image is inserted and the image url of the route is updated accordingly.<br /><br />
        /// **If a new image is not provided**, the old image remains still.<br /><br /><br />
        /// **If provided an ID of a service center, which is not associated with the route**, the route is updated with new service center. The service center is updated accordingly.<br /><br /><br />
        /// ***Parameters:***<br /><br />
        /// `Image` : Must be a valid image (possible extensions: ".jpg", ".jpeg", ".png", ".gif", ".bmp"). The size must be less than 500 Kb. Not required field.<br /><br />
        /// `Request` : Route represented as a JSON object.<br /><br /><br />
        /// **Request parameters:**<br /><br />
        /// `RouteId`: Represents ID of a route to be updated. Must be a valid GUID.<br /><br />
        /// `ServiceCenterId`: Represents ID of a service center to be associated with this route. Must be a valid GUID.<br /><br />
        /// `RouteNumber` : Represents number of the route. Must be an integer with the minimum value of 1.<br /><br />
        /// `Description` : Represents description of the route. Must be a string with the maximum length of 2000 characters. Not required field.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="command">**The update route command.**</param>  
        /// <response code="204">Successfully updated an existing route.</response>
        /// <response code="400">***Bad request.*** The provided data is invalid or missing.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** Route or Service Center with the provided IDs are not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpPut]
        [HasPermission(Permission.ModifyApplicationData)]
        [Consumes(Multipart.FormData)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateRoute(
            [FromForm] UpdateRouteCommandWrapper command)
        {
            var commandResult = await _sender.Send(command.ToCommand());

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <summary>
        /// Deletes a route using its ID.
        /// </summary>
        /// <remarks>
        /// **The request must include the ID of a route.**<br /><br />
        /// **If succesfully removed a route**, its image URI is not valid anymore.<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `RouteId` : Must be a valid GUID representing ID of a route.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="routeId">**The ID of the route to be deleted.**</param>
        /// <response code="204">Successfully deleted the route.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** Route with the provided ID is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpDelete("{routeId:guid}")]
        [HasPermission(Permission.ModifyApplicationData)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteRoute(
            [FromRoute] Guid routeId)
        {
            var commandResult = await _sender.Send(new DeleteRouteCommand(routeId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }


        #endregion
    }
}
