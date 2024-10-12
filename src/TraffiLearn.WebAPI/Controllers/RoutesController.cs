using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TraffiLearn.Application.Routes.DTO;
using TraffiLearn.Application.Routes.Queries.GetById;
using TraffiLearn.Infrastructure.Authentication;
using TraffiLearn.WebAPI.Extensions;
using TraffiLearn.WebAPI.Swagger;

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
    }
}
