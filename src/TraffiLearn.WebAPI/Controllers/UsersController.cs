﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using TraffiLearn.Application.Comments.DTO;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Application.Users.Commands.ChangeSubscriptionPlan;
using TraffiLearn.Application.Users.Commands.DowngradeAccount;
using TraffiLearn.Application.Users.DTO;
using TraffiLearn.Application.Users.Queries.GetAllAdmins;
using TraffiLearn.Application.Users.Queries.GetAllUsers;
using TraffiLearn.Application.Users.Queries.GetCurrentUserInfo;
using TraffiLearn.Application.Users.Queries.GetLoggedInUserComments;
using TraffiLearn.Application.Users.Queries.GetUserComments;
using TraffiLearn.Application.Users.Queries.GetUserDislikedQuestions;
using TraffiLearn.Application.Users.Queries.GetUserLikedQuestions;
using TraffiLearn.Infrastructure.Authentication;
using TraffiLearn.WebAPI.Extensions;
using TraffiLearn.WebAPI.Swagger;

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
        /// Gets info of the current (logged in) in user.
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
        /// Changes a subscription plan of an authenticated user.
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
        [HttpPut("change-subscription/{planId:guid}")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeSubscriptionPlan(
            [FromRoute] Guid planId)
        {
            var commandResult = await _sender.Send(
                new ChangeSubscriptionPlanCommand(planId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }


        #endregion
    }
}
