using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TraffiLearn.Application.UseCases.SubscriptionPlans.Commands.Create;
using TraffiLearn.Application.UseCases.SubscriptionPlans.Commands.Delete;
using TraffiLearn.Application.UseCases.SubscriptionPlans.Commands.Update;
using TraffiLearn.Application.UseCases.SubscriptionPlans.DTO;
using TraffiLearn.Application.UseCases.SubscriptionPlans.Queries.GetAll;
using TraffiLearn.Application.UseCases.SubscriptionPlans.Queries.GetById;
using TraffiLearn.Infrastructure.Authentication;
using TraffiLearn.WebAPI.Extensions;
using TraffiLearn.WebAPI.Swagger;

namespace TraffiLearn.WebAPI.Controllers
{
    [Route("api/subscription-plans")]
    [ApiController]
    public class SubscriptionPlansController : ControllerBase
    {
        private readonly ISender _sender;

        public SubscriptionPlansController(ISender sender)
        {
            _sender = sender;
        }

        #region Queries


        /// <include file='Documentation/SubscriptionPlansControllerDocs.xml' path='doc/members/member[@name="M:GetSubscriptionPlanById"]/*'/>
        [HttpGet("{planId:guid}")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(SubscriptionPlanResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSubscriptionPlanById(
            [FromRoute] Guid planId)
        {
            var queryResult = await _sender.Send(new GetSubscriptionPlanByIdQuery(planId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <include file='Documentation/SubscriptionPlansControllerDocs.xml' path='doc/members/member[@name="M:GetAllSubscriptionPlans"]/*'/>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<SubscriptionPlanResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllSubscriptionPlans()
        {
            var queryResult = await _sender.Send(new GetAllSubscriptionPlansQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }


        #endregion

        #region Commands


        /// <include file='Documentation/SubscriptionPlansControllerDocs.xml' path='doc/members/member[@name="M:CreateSubscriptionPlan"]/*'/>
        [HttpPost]
        [HasPermission(Permission.ManageSubscriptionPlans)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateSubscriptionPlan(
            [FromBody] CreateSubscriptionPlanCommand command)
        {
            var commandResult = await _sender.Send(command);

            if (commandResult.IsSuccess)
            {
                return CreatedAtAction(
                    actionName: nameof(GetSubscriptionPlanById),
                    routeValues: new { planId = commandResult.Value },
                    value: commandResult.Value);
            }

            return commandResult.ToProblemDetails();
        }

        /// <include file='Documentation/SubscriptionPlansControllerDocs.xml' path='doc/members/member[@name="M:DeleteSubscriptionPlan"]/*'/>
        [HttpDelete("{planId:guid}")]
        [HasPermission(Permission.ManageSubscriptionPlans)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteSubscriptionPlan(
            [FromRoute] Guid planId)
        {
            var commandResult = await _sender.Send(new DeleteSubscriptionPlanCommand(planId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <include file='Documentation/SubscriptionPlansControllerDocs.xml' path='doc/members/member[@name="M:UpdateSubscriptionPlan"]/*'/>
        [HttpPut]
        [HasPermission(Permission.ManageSubscriptionPlans)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateSubscriptionPlan(
            [FromBody] UpdateSubscriptionPlanCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }


        #endregion
    }
}
