using MediatR;
using Microsoft.AspNetCore.Mvc;
using TraffiLearn.Application.Queries.Users.GetUserComments;
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


        [HttpGet("{userId:guid}/comments")]
        public async Task<IActionResult> GetUserComments(Guid userId)
        {
            var queryResult = await _sender.Send(new GetUserCommentsQuery(userId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }


        #endregion
    }
}
