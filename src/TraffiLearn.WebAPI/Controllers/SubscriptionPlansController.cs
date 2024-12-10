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

        /// <summary>
        /// Gets all subscription plans from the storage.
        /// </summary>
        /// <response code="200">Successfully retrieved all subscription plans. Returns a list of subscription plans.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
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


        /// <summary>
        /// Creates a new subscription plan.
        /// </summary>
        /// <remarks>
        /// ***Body Parameters:***<br /><br />
        /// `Tier`: The tier of the subscription plan. Must not be empty or whitespace. Maximum length: 50.<br /><br />
        /// `Description`: The description of the subscription plan. Must not be empty or whitespace. Maximum length: 500.<br /><br />
        /// `Price`: The price of the subscription plan. Consists of two properties: <br />
        /// - `Amount`: The amount of the price. Must be a decimal number greater than 0.<br />
        /// - `Currency`: The currency of the price. Must be a valid currency code. Available currencies: <br />
        /// *USD*, *EUR*, *GBP*, *CAD*, *AUD*, *JPY*, *CNY*, *INR*, *CHF*, *SEK*, *NZD*, *ZAR*, *SGD*, *HKD*, *NOK*, *MXN*, *BRL*, *KRW*, *TRY* <br /><br />
        /// `RenewalPeriod`: The renewal period of the subscription plan. Must be a valid renewal period. Consists of two properties: <br />
        /// - `Interval`: The interval of the renewal period. Must be a valid int greater than 0. <br />
        /// - `Type`: The type of the renewal period. Must be a valid renewal period type. Available renewal period types: <br />
        /// *Days*, *Weeks*, *Months*, *Years* <br /><br />
        /// `Features`: List of features of the subscription plan. Must not be empty. Maximum number of features: 20. Each feature must be unique. <br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="command">**The create subscription plan command.**</param>
        /// <response code="201">Successfully created a new subscription plan. Returns ID of a newly created subscription plan.</response>
        /// <response code="400">***Bad request.*** The provided data is invalid or missing.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpPost]
        [HasPermission(Permission.ModifySubscriptionPlans)]
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

        /// <summary>
        /// Deletes a subscription plan using its ID.
        /// </summary>
        /// <remarks>
        /// **The request must include the ID of a subscription plan.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `SubscriptionPlanId` : Must be a valid GUID representing ID of a subscription plan.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="planId">**The ID of the subscription plan to be deleted.**</param>
        /// <response code="204">Successfully deleted the subscription plan.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** Subscription plan with the provided id is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.ModifySubscriptionPlans)]
        [HttpDelete("{planId:guid}")]
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

        /// <summary>
        /// Updates an existing subscription plan.
        /// </summary>
        /// <remarks>
        /// ***Body parameters:***<br /><br />
        /// `SubscriptionPlanId`: ID of the subscription plan to update. Must be a valid GUID. <br /><br />
        /// `Tier`: The tier of the subscription plan. Must not be empty or whitespace.Maximum length: 50.<br /><br />
        /// `Description`: The description of the subscription plan. Must not be empty or whitespace. Maximum length: 500.<br /><br />
        /// `Price`: The price of the subscription plan. Consists of two properties: <br />
        /// - `Amount`: The amount of the price. Must be a decimal number greater than 0.<br />
        /// - `Currency`: The currency of the price. Must be a valid currency code. Available currencies: <br />
        /// *USD*, *EUR*, *GBP*, *CAD*, *AUD*, *JPY*, *CNY*, *INR*, *CHF*, *SEK*, *NZD*, *ZAR*, *SGD*, *HKD*, *NOK*, *MXN*, *BRL*, *KRW*, *TRY* <br /><br />
        /// `RenewalPeriod`: The renewal period of the subscription plan. Must be a valid renewal period. Consists of two properties: <br />
        /// - `Interval`: The interval of the renewal period. Must be a valid int greater than 0. <br />
        /// - `Type`: The type of the renewal period. Must be a valid renewal period type. Available renewal period types: <br />
        /// *Days*, *Weeks*, *Months*, *Years* <br /><br />
        /// `Features`: List of features of the subscription plan. Must not be empty. Maximum number of features: 20. Each feature must be unique. <br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="command">The update subscription plan command.</param>
        /// <response code="204">Successfully updated an existing subscription plan.</response>
        /// <response code="400">***Bad request.*** The provided data is invalid or missing.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** Subscription plan with the ID is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.ModifySubscriptionPlans)]
        [HttpPut]
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
