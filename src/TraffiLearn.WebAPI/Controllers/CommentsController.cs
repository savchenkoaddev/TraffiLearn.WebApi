using MediatR;
using Microsoft.AspNetCore.Mvc;
using TraffiLearn.Application.Comments.Commands.DeleteComment;
using TraffiLearn.Application.Comments.Commands.Reply;
using TraffiLearn.Application.Comments.Commands.UpdateComment;
using TraffiLearn.Application.Comments.Queries.GetCommentReplies;
using TraffiLearn.Application.Users.Commands.DislikeComment;
using TraffiLearn.Application.Users.Commands.LikeComment;
using TraffiLearn.Application.Users.Commands.RemoveCommentDislike;
using TraffiLearn.Application.Users.Commands.RemoveCommentLike;
using TraffiLearn.Infrastructure.Authentication;
using TraffiLearn.WebAPI.Extensions;

namespace TraffiLearn.WebAPI.Controllers
{
    [HasPermission(Permission.AccessData)]
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

        [HasPermission(Permission.ModifyData)]
        [HttpPut]
        public async Task<IActionResult> UpdateComment(UpdateCommentCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HasPermission(Permission.ModifyData)]
        [HttpDelete("{commentId:guid}")]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            var commandResult = await _sender.Send(new DeleteCommentCommand(commentId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HttpPut("{commentId:guid}/like")]
        public async Task<IActionResult> LikeComment(Guid commentId)
        {
            var commandResult = await _sender.Send(new LikeCommentCommand(commentId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HttpPut("{commentId:guid}/dislike")]
        public async Task<IActionResult> DislikeComment(Guid commentId)
        {
            var commandResult = await _sender.Send(new DislikeCommentCommand(commentId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HttpPut("{commentId:guid}/remove-like")]
        public async Task<IActionResult> RemoveCommentLike(Guid commentId)
        {
            var commandResult = await _sender.Send(new RemoveCommentLikeCommand(commentId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        [HttpPut("{commentId:guid}/remove-dislike")]
        public async Task<IActionResult> RemoveCommentDislike(Guid commentId)
        {
            var commandResult = await _sender.Send(new RemoveCommentDislikeCommand(commentId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }


        #endregion
    }
}
