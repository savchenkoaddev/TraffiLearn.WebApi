using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Net.Mime;
using TraffiLearn.Application.UseCases.CanceledSubscriptions.DTO;
using TraffiLearn.Application.UseCases.Comments.DTO;
using TraffiLearn.Application.UseCases.Questions.DTO;
using TraffiLearn.Application.UseCases.Transactions.DTO;
using TraffiLearn.Application.UseCases.Users.Commands.CancelSubscription;
using TraffiLearn.Application.UseCases.Users.Commands.DowngradeAccount;
using TraffiLearn.Application.UseCases.Users.Commands.RequestChangeSubscriptionPlan;
using TraffiLearn.Application.UseCases.Users.Commands.RequestRenewSubscriptionPlan;
using TraffiLearn.Application.UseCases.Users.DTO;
using TraffiLearn.Application.UseCases.Users.Queries.GetAllAdmins;
using TraffiLearn.Application.UseCases.Users.Queries.GetAllUsers;
using TraffiLearn.Application.UseCases.Users.Queries.GetCurrentUserCanceledSubscriptions;
using TraffiLearn.Application.UseCases.Users.Queries.GetCurrentUserComments;
using TraffiLearn.Application.UseCases.Users.Queries.GetCurrentUserInfo;
using TraffiLearn.Application.UseCases.Users.Queries.GetCurrentUserTransactions;
using TraffiLearn.Application.UseCases.Users.Queries.GetUserComments;
using TraffiLearn.Application.UseCases.Users.Queries.GetUserDislikedQuestions;
using TraffiLearn.Application.UseCases.Users.Queries.GetUserLikedQuestions;
using TraffiLearn.Infrastructure.Authentication;
using TraffiLearn.Infrastructure.Extensions.DI;
using TraffiLearn.WebAPI.Factories;
using TraffiLearn.WebAPI.Swagger;

namespace TraffiLearn.WebAPI.Controllers
{
    [Route("api/users")]
    [EnableRateLimiting(RateLimitingExtensions.DefaultPolicyName)]
    [ApiController]
    public sealed class UsersController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly ProblemDetailsFactory _problemDetailsFactory;

        public UsersController(
            ISender sender,
            ProblemDetailsFactory problemDetailsFactory)
        {
            _sender = sender;
            _problemDetailsFactory = problemDetailsFactory;
        }

        #region Queries


        /// <include file='Documentation/UsersControllerDocs.xml' path='doc/members/member[@name="M:GetCurrentUserInfo"]/*'/>
        [HttpGet("current")]
        [HasPermission(Permission.AuthenticatedUser)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(CurrentUserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCurrentUserInfo()
        {
            var queryResult = await _sender.Send(new GetCurrentUserInfoQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : _problemDetailsFactory.ToProblemDetails(queryResult);
        }

        /// <include file='Documentation/UsersControllerDocs.xml' path='doc/members/member[@name="M:GetAllUsers"]/*'/>
        [HttpGet]
        [HasPermission(Permission.ViewUsersData)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<UserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllUsers()
        {
            var queryResult = await _sender.Send(new GetAllUsersQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : _problemDetailsFactory.ToProblemDetails(queryResult);
        }

        /// <include file='Documentation/UsersControllerDocs.xml' path='doc/members/member[@name="M:GetAllAdmins"]/*'/>
        [HttpGet("admins")]
        [HasPermission(Permission.ViewAdminsData)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<UserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAdmins()
        {
            var queryResult = await _sender.Send(new GetAllAdminsQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : _problemDetailsFactory.ToProblemDetails(queryResult);
        }

        /// <include file='Documentation/UsersControllerDocs.xml' path='doc/members/member[@name="M:GetUserComments"]/*'/>
        [HttpGet("{userId:guid}/comments")]
        [HasPermission(Permission.ViewUsersData)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(IEnumerable<CommentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserComments(
            [FromRoute] Guid userId)
        {
            var queryResult = await _sender.Send(new GetUserCommentsQuery(userId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : _problemDetailsFactory.ToProblemDetails(queryResult);
        }

        /// <include file='Documentation/UsersControllerDocs.xml' path='doc/members/member[@name="M:GetUserLikedQuestions"]/*'/>
        [HttpGet("{userId:guid}/liked-questions")]
        [HasPermission(Permission.ViewUsersData)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(IEnumerable<QuestionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserLikedQuestions(
            [FromRoute] Guid userId)
        {
            var queryResult = await _sender.Send(new GetUserLikedQuestionsQuery(userId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : _problemDetailsFactory.ToProblemDetails(queryResult);
        }

        /// <include file='Documentation/UsersControllerDocs.xml' path='doc/members/member[@name="M:GetUserDislikedQuestions"]/*'/>
        [HttpGet("{userId:guid}/disliked-questions")]
        [HasPermission(Permission.ViewUsersData)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(IEnumerable<QuestionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserDislikedQuestions(
            [FromRoute] Guid userId)
        {
            var queryResult = await _sender.Send(new GetUserDislikedQuestionsQuery(userId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : _problemDetailsFactory.ToProblemDetails(queryResult);
        }

        /// <include file='Documentation/UsersControllerDocs.xml' path='doc/members/member[@name="M:GetCurrentUserComments"]/*'/>
        [HttpGet("current/comments")]
        [HasPermission(Permission.AuthenticatedUser)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<CommentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCurrentUserComments()
        {
            var queryResult = await _sender.Send(new GetCurrentUserCommentsQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : _problemDetailsFactory.ToProblemDetails(queryResult);
        }

        /// <include file='Documentation/UsersControllerDocs.xml' path='doc/members/member[@name="M:GetCurrentUserTransactions"]/*'/>
        [HttpGet("current/transactions")]
        [HasPermission(Permission.AuthenticatedUser)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<TransactionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCurrentUserTransactions()
        {
            var queryResult = await _sender.Send(new GetCurrentUserTransactionsQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : _problemDetailsFactory.ToProblemDetails(queryResult);
        }

        /// <include file='Documentation/UsersControllerDocs.xml' path='doc/members/member[@name="M:GetCurrentUserCanceledSubscriptions"]/*'/>
        [HttpGet("current/canceled-subscriptions")]
        [HasPermission(Permission.AuthenticatedUser)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<CanceledSubscriptionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCurrentUserCanceledSubscriptions()
        {
            var queryResult = await _sender.Send(new GetCurrentUserCanceledSubscriptionsQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : _problemDetailsFactory.ToProblemDetails(queryResult);
        }


        #endregion

        #region Commands


        /// <include file='Documentation/UsersControllerDocs.xml' path='doc/members/member[@name="M:DowngradeAccount"]/*'/>
        [HasPermission(Permission.ManageAccountStatuses)]
        [HttpPut("{userId:guid}/downgrade")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DowngradeAccount(
            [FromRoute] Guid userId)
        {
            var commandResult = await _sender.Send(new DowngradeAccountCommand(userId));

            return commandResult.IsSuccess ? NoContent() : _problemDetailsFactory.ToProblemDetails(commandResult);
        }

        /// <include file='Documentation/UsersControllerDocs.xml' path='doc/members/member[@name="M:RequestChangeSubscriptionPlan"]/*'/>
        [HasPermission(Permission.AuthenticatedUser)]
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
                : _problemDetailsFactory.ToProblemDetails(commandResult);
        }

        /// <include file='Documentation/UsersControllerDocs.xml' path='doc/members/member[@name="M:RenewSubscription"]/*'/>
        [HasPermission(Permission.AuthenticatedUser)]
        [HttpPut("request-renew-subscription")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(DateTime), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RenewSubscription()
        {
            var commandResult = await _sender.Send(
                new RequestRenewSubscriptionPlanCommand());

            return commandResult.IsSuccess
                ? Ok(commandResult.Value.ToString())
                : _problemDetailsFactory.ToProblemDetails(commandResult);
        }

        /// <include file='Documentation/UsersControllerDocs.xml' path='doc/members/member[@name="M:CancelSubscription"]/*'/>
        [HasPermission(Permission.AuthenticatedUser)]
        [HttpPut("cancel-subscription")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CancelSubscription(
            [FromBody] CancelSubscriptionCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? NoContent() : _problemDetailsFactory.ToProblemDetails(commandResult);
        }


        #endregion
    }
}
