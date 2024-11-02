using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TraffiLearn.Application.SubscriptionPlans.DTO;
using TraffiLearn.Application.SubscriptionPlans.Queries.GetById;
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

        /// <summary>
        /// Gets a subscription plan with a specific ID.
        /// </summary>
        /// <remarks>
        /// **The request must include an ID of a plan to get.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `PlanId` : Must be a valid GUID representing ID of a subscription plan.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.<br /><br />
        /// </remarks>
        /// <param name="planId">**The ID of a subscription plan to be retrieved**</param>
        /// <response code="200">Successfully retrieved the subscription plan with the provided ID</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="404">***Not found.*** No subscription plan exists with the provided ID.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
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
    }
}
