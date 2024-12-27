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
    [HasPermission(Permission.AuthenticatedUser)]
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


        /// <include file='Documentation/ServiceCentersControllerDocs.xml' path='doc/members/member[@name="M:GetAllServiceCenters"]/*'/>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<ServiceCenterResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllServiceCenters()
        {
            var queryResult = await _sender.Send(new GetAllServiceCentersQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <include file='Documentation/ServiceCentersControllerDocs.xml' path='doc/members/member[@name="M:GetServiceCenterById"]/*'/>
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

        /// <include file='Documentation/ServiceCentersControllerDocs.xml' path='doc/members/member[@name="M:GetServiceCentersByRegionId"]/*'/>
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


        /// <include file='Documentation/ServiceCentersControllerDocs.xml' path='doc/members/member[@name="M:CreateServiceCenter"]/*'/>
        [HttpPost]
        [HasPermission(Permission.ModifyApplicationData)]
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

        /// <include file='Documentation/ServiceCentersControllerDocs.xml' path='doc/members/member[@name="M:UpdateServiceCenter"]/*'/>
        [HttpPut]
        [HasPermission(Permission.ModifyApplicationData)]
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

        /// <include file='Documentation/ServiceCentersControllerDocs.xml' path='doc/members/member[@name="M:DeleteServiceCenter"]/*'/>
        [HttpDelete("{serviceCenterId:guid}")]
        [HasPermission(Permission.ModifyApplicationData)]
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
