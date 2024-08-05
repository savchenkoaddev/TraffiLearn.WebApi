using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraffiLearn.Application.Queries.Users.GetLoggedInUserComments;
using TraffiLearn.Application.Queries.Users.GetUserComments;
using TraffiLearn.WebAPI.Extensions;

namespace TraffiLearn.WebAPI.Controllers
{
    [Authorize]
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


        [HttpGet("{userId:guid}/comments")]
        public async Task<IActionResult> GetUserComments(Guid userId)
        {
            var queryResult = await _sender.Send(new GetUserCommentsQuery(userId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }

        [HttpGet("current/comments")]
        public async Task<IActionResult> GetLoggedInUserComments()
        {
            var queryResult = await _sender.Send(new GetLoggedInUserCommentsQuery());

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }


        #endregion
    }
}
