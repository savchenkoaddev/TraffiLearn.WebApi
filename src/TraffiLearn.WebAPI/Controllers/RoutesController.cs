using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Net.Mime;
using TraffiLearn.Application.UseCases.Routes.Commands.Delete;
using TraffiLearn.Application.UseCases.Routes.DTO;
using TraffiLearn.Application.UseCases.Routes.Queries.GetById;
using TraffiLearn.Application.UseCases.Routes.Queries.GetByServiceCenterId;
using TraffiLearn.Infrastructure.Authentication;
using TraffiLearn.Infrastructure.Extensions.DI;
using TraffiLearn.WebAPI.CommandWrappers.CreateRoute;
using TraffiLearn.WebAPI.CommandWrappers.UpdateRoute;
using TraffiLearn.WebAPI.Factories;
using TraffiLearn.WebAPI.Swagger;
using static System.Net.Mime.MediaTypeNames;

namespace TraffiLearn.WebAPI.Controllers
{
    [HasPermission(Permission.AuthenticatedUser)]
    [EnableRateLimiting(RateLimitingExtensions.DefaultPolicyName)]
    [ApiController]
    [Route("api/routes")]
    public class RoutesController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly ProblemDetailsFactory _problemDetailsFactory;

        public RoutesController(
            ISender sender,
            ProblemDetailsFactory problemDetailsFactory)
        {
            _sender = sender;
            _problemDetailsFactory = problemDetailsFactory;
        }

        #region Queries


        /// <include file='Documentation/RoutesControllerDocs.xml' path='doc/members/member[@name="M:GetRouteById"]/*'/>
        [HttpGet("{routeId:guid}")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(RouteResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRouteById(
            [FromRoute] Guid routeId)
        {
            var queryResult = await _sender.Send(new GetRouteByIdQuery(routeId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : _problemDetailsFactory.ToProblemDetails(queryResult);
        }

        /// <include file='Documentation/RoutesControllerDocs.xml' path='doc/members/member[@name="M:GetRoutesByServiceCenterId"]/*'/>
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

            return queryResult.IsSuccess ? Ok(queryResult.Value) : _problemDetailsFactory.ToProblemDetails(queryResult);
        }


        #endregion

        #region Commands


        /// <include file='Documentation/RoutesControllerDocs.xml' path='doc/members/member[@name="M:CreateRoute"]/*'/>
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

            return _problemDetailsFactory.ToProblemDetails(commandResult);
        }

        /// <include file='Documentation/RoutesControllerDocs.xml' path='doc/members/member[@name="M:UpdateRoute"]/*'/>
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

            return commandResult.IsSuccess ? NoContent() : _problemDetailsFactory.ToProblemDetails(commandResult);
        }

        /// <include file='Documentation/RoutesControllerDocs.xml' path='doc/members/member[@name="M:DeleteRoute"]/*'/>
        [HttpDelete("{routeId:guid}")]
        [HasPermission(Permission.ModifyApplicationData)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteRoute(
            [FromRoute] Guid routeId)
        {
            var commandResult = await _sender.Send(new DeleteRouteCommand(routeId));

            return commandResult.IsSuccess ? NoContent() : _problemDetailsFactory.ToProblemDetails(commandResult);
        }


        #endregion
    }
}
