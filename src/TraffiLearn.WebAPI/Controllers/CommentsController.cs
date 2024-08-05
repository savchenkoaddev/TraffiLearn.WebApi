using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraffiLearn.Application.Commands.Comments.DeleteComment;
using TraffiLearn.Application.Commands.Comments.Reply;
using TraffiLearn.Application.Commands.Comments.UpdateComment;
using TraffiLearn.Application.Queries.Comments.GetCommentReplies;
using TraffiLearn.WebAPI.Extensions;

namespace TraffiLearn.WebAPI.Controllers
{
    [Authorize]
    [Route("api/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ISender _sender;

        public CommentsController(ISender sender)
        {
            _sender = sender;
        }

        #region Queries


        [HttpGet("{commentId:guid}/replies")]
        public async Task<IActionResult> GetCommentReplies(Guid commentId)
        {
            var queryResult = await _sender.Send(new GetCommentsRepliesQuery(commentId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }


        #endregion

        #region Commands


        [HttpPost("reply")]
        public async Task<IActionResult> Reply(ReplyCommand replyCommand)
        {
            var commandResult = await _sender.Send(replyCommand);

            return commandResult.IsSuccess ? Created() : commandResult.ToProblemDetails();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateComment(UpdateCommentCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HttpDelete("{commentId:guid}")]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            var commandResult = await _sender.Send(new DeleteCommentCommand(commentId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }


        #endregion
    }
}
