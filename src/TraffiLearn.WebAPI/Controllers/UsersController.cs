using MediatR;
using Microsoft.AspNetCore.Mvc;
using TraffiLearn.Application.Commands.Users.DowngradeAccount;
using TraffiLearn.Application.Queries.Users.GetLoggedInUserComments;
using TraffiLearn.Application.Queries.Users.GetUserComments;
using TraffiLearn.Application.Queries.Users.GetUserDislikedQuestions;
using TraffiLearn.Application.Queries.Users.GetUserLikedQuestions;
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


        [HasPermission(Permission.DowngradeAccount)]
        [HttpPut("{userId:guid}/downgrade")]
        public async Task<IActionResult> DowngradeAccount(Guid userId)
        {
            var commandResult = await _sender.Send(new DowngradeAccountCommand(userId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }


        #endregion
    }
}
