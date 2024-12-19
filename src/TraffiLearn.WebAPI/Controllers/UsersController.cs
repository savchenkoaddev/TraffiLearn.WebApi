using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TraffiLearn.Application.UseCases.Comments.DTO;
using TraffiLearn.Application.UseCases.Questions.DTO;
using TraffiLearn.Application.UseCases.Users.Commands.CancelSubscription;
using TraffiLearn.Application.UseCases.Users.Commands.RequestChangeSubscriptionPlan;
using TraffiLearn.Application.UseCases.Users.Commands.DowngradeAccount;
using TraffiLearn.Application.UseCases.Users.Commands.RenewSubscriptionPlan;
using TraffiLearn.Application.UseCases.Users.DTO;
using TraffiLearn.Application.UseCases.Users.Queries.GetAllAdmins;
using TraffiLearn.Application.UseCases.Users.Queries.GetAllUsers;
using TraffiLearn.Application.UseCases.Users.Queries.GetCurrentUserInfo;
using TraffiLearn.Application.UseCases.Users.Queries.GetLoggedInUserComments;
using TraffiLearn.Application.UseCases.Users.Queries.GetUserComments;
using TraffiLearn.Application.UseCases.Users.Queries.GetUserDislikedQuestions;
using TraffiLearn.Application.UseCases.Users.Queries.GetUserLikedQuestions;
using TraffiLearn.Infrastructure.Authentication;
using TraffiLearn.WebAPI.Extensions;
using TraffiLearn.WebAPI.Swagger;
using TraffiLearn.Application.UseCases.Users.Queries.GetCurrentUserTransactions;
using TraffiLearn.Application.UseCases.Transactions.DTO;

namespace TraffiLearn.WebAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public sealed class UsersController : ControllerBase
    {
        private readonly ISender _sender;

        public UsersController(ISender sender)
        {
            _sender = sender;
        }

        #region Queries


        /// <summary>
        /// Gets information about the authenticated user.
        /// </summary>
        /// <remarks>
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.
        /// </remarks>
        /// <response code="200">Successfully retrieved logged in user comments. Returns logged in user comments.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.AccessData)]
        [HttpGet("current")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(CurrentUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCurrentUserInfo()
        {
            var queryResult = await _sender.Send(new GetCurrentUserInfoQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <summary>
        /// Gets all users from the storage.
        /// </summary>
        /// <remarks>
        ///  **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <response code="200">Successfully retrieved all users. Returns a list of users.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.AccessSpecificUserData)]
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<UserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllUsers()
        {
            var queryResult = await _sender.Send(new GetAllUsersQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <summary>
        /// Gets all admins from the storage.
        /// </summary>
        /// <remarks>
        ///  **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` role can perform this action.<br /><br />
        /// </remarks>
        /// <response code="200">Successfully retrieved all admins. Returns a list of admins.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.AccessSpecificAdminData)]
        [HttpGet("admins")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<UserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAdmins()
        {
            var queryResult = await _sender.Send(new GetAllAdminsQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <summary>
        /// Gets all comments of a user with a specific ID.
        /// </summary>
        /// <remarks>
        /// **The request must include an ID of a user.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `UserId` : Must be a valid GUID representing ID of a user.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="userId">**The ID of a user, whose comments are being retrieved.**</param>
        /// <response code="200">Successfully retrieved user comments with the provided user ID. Returns user comments.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** No user exists with the provided ID.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.AccessSpecificUserData)]
        [HttpGet("{userId:guid}/comments")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(IEnumerable<CommentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserComments(
            [FromRoute] Guid userId)
        {
            var queryResult = await _sender.Send(new GetUserCommentsQuery(userId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <summary>
        /// Gets all liked questions of a user with a specific ID.
        /// </summary>
        /// <remarks>
        /// **The request must include an ID of a user.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `UserId` : Must be a valid GUID representing ID of a user.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="userId">**The ID of a user, whose liked questions are being retrieved.**</param>
        /// <response code="200">Successfully retrieved user liked questions with the provided user ID. Returns user liked questions.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** No user exists with the provided ID.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.AccessSpecificUserData)]
        [HttpGet("{userId:guid}/liked-questions")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(IEnumerable<QuestionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserLikedQuestions(
            [FromRoute] Guid userId)
        {
            var queryResult = await _sender.Send(new GetUserLikedQuestionsQuery(userId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <summary>
        /// Gets all disliked questions of a user with a specific ID.
        /// </summary>
        /// <remarks>
        /// **The request must include an ID of a user.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `UserId` : Must be a valid GUID representing ID of a user.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="userId">**The ID of a user, whose disliked questions are being retrieved.**</param>
        /// <response code="200">Successfully retrieved user disliked questions with the provided user ID. Returns user disliked questions.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** No user exists with the provided ID.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.AccessSpecificUserData)]
        [HttpGet("{userId:guid}/disliked-questions")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(IEnumerable<QuestionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserDislikedQuestions(
            [FromRoute] Guid userId)
        {
            var queryResult = await _sender.Send(new GetUserDislikedQuestionsQuery(userId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <summary>
        /// Gets all comments of the currently logged in user.
        /// </summary>
        /// <remarks>
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.
        /// </remarks>
        /// <response code="200">Successfully retrieved logged in user comments. Returns logged in user comments.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.AccessData)]
        [HttpGet("current/comments")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<CommentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLoggedInUserComments()
        {
            var queryResult = await _sender.Send(new GetLoggedInUserCommentsQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        /// <summary>
        /// Gets all transactions of the authenticated user.
        /// </summary>
        /// <remarks>
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.
        /// </remarks>
        /// <response code="200">Successfully retrieved the current user's transactions. Returns list of transactions.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.AccessData)]
        [HttpGet("current/transactions")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<TransactionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCurrentUserTransactions()
        {
            var queryResult = await _sender.Send(new GetCurrentUserTransactionsQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }


        #endregion

        #region Commands


        /// <summary>
        /// Downgrades the account of a user with a specific ID.
        /// </summary>
        /// <remarks>
        /// **If account getting downgraded has role `Admin`**, user will be downgraded to the role `RegularUser`.<br /><br />
        /// **`RegularUser` and `Owner` roles cannot be downgraded.**<br /><br />
        /// **The request must include an ID of a user.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `UserId` : Must be a valid GUID representing ID of a user.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="userId">**The ID of user, who is being downgraded.**</param>
        /// <response code="204">Successfully downgraded the user with the provided ID.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** No user exists with the provided ID.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.DowngradeAccounts)]
        [HttpPut("{userId:guid}/downgrade")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DowngradeAccount(
            [FromRoute] Guid userId)
        {
            var commandResult = await _sender.Send(new DowngradeAccountCommand(userId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <summary>
        /// Requests a change for a subscription plan of an authenticated user.
        /// </summary>
        /// <remarks>
        /// **If a user has the same subscription plan**, an error will be returned.<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `PlanId` : Must be a valid GUID representing ID of a subscription plan.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.<br /><br />
        /// </remarks>
        /// <param name="planId">**The ID of a subscription plan.**</param>
        /// <response code="204">Successfully changed a subscription plan.</response>
        /// <response code="400">***Bad request.*** The user has the same subscription plan.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="404">***Not found.*** No subscription plan exists with the provided ID.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.AccessData)]
        [HttpPut("request-change-subscription/{planId:guid}")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RequestChangeSubscriptionPlan(
            [FromRoute] Guid planId)
        {
            var commandResult = await _sender.Send(
                new RequestChangeSubscriptionPlanCommand(planId));

            return commandResult.IsSuccess 
                ? Ok(commandResult.Value.ToString()) 
                : commandResult.ToProblemDetails();
        }

        /// <summary>
        /// Renews a subscription of an authenticated user.
        /// </summary>
        /// <remarks>
        /// **If a user does not have any subscription plan**, an error will be returned.<br /><br />
        /// **If a user already has a subscription plan, which has not expired yet**, the new renewal extends from the current expiry date.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.<br /><br />
        /// </remarks>
        /// <response code="200">***OK.*** Successfully renewed a subscription. Returns the new expiration date.</response>
        /// <response code="400">***Bad request.*** Violation of some business rules.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.AccessData)]
        [HttpPut("renew-subscription")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(DateTime), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RenewSubscription()
        {
            var commandResult = await _sender.Send(
                new RenewSubscriptionPlanCommand());

            return commandResult.IsSuccess 
                ? Ok(new { ExpirationDate = commandResult.Value }) 
                : commandResult.ToProblemDetails();
        }

        /// <summary>
        /// Cancels a subscription of an authenticated user.
        /// </summary>
        /// <remarks>
        /// **If a user does not have any subscription plan**, an error will be returned.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.<br /><br />
        /// </remarks>
        /// <param name="command">**The cancel subscription command.**</param>
        /// <response code="204">***No Content.*** Successfully canceled a subscription.</response>
        /// <response code="400">***Bad request.*** Violation of some business rules.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HasPermission(Permission.AccessData)]
        [HttpPut("cancel-subscription")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CancelSubscription(
            [FromBody] CancelSubscriptionCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }


        #endregion
    }
}
