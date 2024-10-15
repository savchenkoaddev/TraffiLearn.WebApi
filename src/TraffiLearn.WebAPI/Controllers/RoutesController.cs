using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TraffiLearn.Application.Routes.DTO;
using TraffiLearn.Application.Routes.Queries.GetById;
using TraffiLearn.Infrastructure.Authentication;
using TraffiLearn.WebAPI.CommandWrappers.CreateRoute;
using TraffiLearn.WebAPI.Extensions;
using TraffiLearn.WebAPI.Swagger;
using static System.Net.Mime.MediaTypeNames;

namespace TraffiLearn.WebAPI.Controllers
{
    [HasPermission(Permission.AccessData)]
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
        /// `ServiceCenterId`: Represents ID of the service center to be associated with this route. Must be a valid GUID.<br /><br />
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
        [HasPermission(Permission.ModifyData)]
        [HttpPost]
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


        #endregion
    }
}
