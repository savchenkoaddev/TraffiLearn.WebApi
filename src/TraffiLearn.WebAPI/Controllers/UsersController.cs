using MediatR;
using Microsoft.AspNetCore.Mvc;
using TraffiLearn.Application.Users.Commands.DowngradeAccount;
using TraffiLearn.Application.Users.Queries.GetAllUsers;
using TraffiLearn.Application.Users.Queries.GetLoggedInUserComments;
using TraffiLearn.Application.Users.Queries.GetUserComments;
using TraffiLearn.Application.Users.Queries.GetUserDislikedQuestions;
using TraffiLearn.Application.Users.Queries.GetUserLikedQuestions;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Infrastructure.Authentication;
using TraffiLearn.WebAPI.Extensions;

namespace TraffiLearn.WebAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ISender _sender;

        public UsersController(ISender sender)
        {
            _sender = sender;
        }

        #region Queries

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var queryResult = await _sender.Send(new GetAllUsersQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        [HasPermission(Permission.AccessSpecificUserData)]
        [HttpGet("{userId:guid}/comments")]
        public async Task<IActionResult> GetUserComments(Guid userId)
        {
            var queryResult = await _sender.Send(new GetUserCommentsQuery(userId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        [HasPermission(Permission.AccessSpecificUserData)]
        [HttpGet("{userId:guid}/liked-questions")]
        public async Task<IActionResult> GetUserLikedQuestions(Guid userId)
        {
            var queryResult = await _sender.Send(new GetUserLikedQuestionsQuery(userId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        [HasPermission(Permission.AccessSpecificUserData)]
        [HttpGet("{userId:guid}/disliked-questions")]
        public async Task<IActionResult> GetUserDislikedQuestions(Guid userId)
        {
            var queryResult = await _sender.Send(new GetUserDislikedQuestionsQuery(userId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        [HasPermission(Permission.AccessData)]
        [HttpGet("current/comments")]
        public async Task<IActionResult> GetLoggedInUserComments()
        {
            var queryResult = await _sender.Send(new GetLoggedInUserCommentsQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }


        #endregion

        #region Commands


        [HasPermission(Permission.DowngradeAccounts)]
        [HttpPut("{userId:guid}/downgrade")]
        public async Task<IActionResult> DowngradeAccount(Guid userId)
        {
            var commandResult = await _sender.Send(new DowngradeAccountCommand(userId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }


        #endregion
    }
}
