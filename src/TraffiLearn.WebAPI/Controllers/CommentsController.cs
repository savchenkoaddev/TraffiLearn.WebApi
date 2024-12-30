using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Net.Mime;
using TraffiLearn.Application.UseCases.Comments.Commands.DeleteComment;
using TraffiLearn.Application.UseCases.Comments.Commands.Reply;
using TraffiLearn.Application.UseCases.Comments.Commands.UpdateComment;
using TraffiLearn.Application.UseCases.Comments.DTO;
using TraffiLearn.Application.UseCases.Comments.Queries.GetCommentReplies;
using TraffiLearn.Application.UseCases.Users.Commands.DislikeComment;
using TraffiLearn.Application.UseCases.Users.Commands.LikeComment;
using TraffiLearn.Application.UseCases.Users.Commands.RemoveCommentDislike;
using TraffiLearn.Application.UseCases.Users.Commands.RemoveCommentLike;
using TraffiLearn.Infrastructure.Authentication;
using TraffiLearn.Infrastructure.Extensions.DI;
using TraffiLearn.WebAPI.Factories;
using TraffiLearn.WebAPI.Swagger;

namespace TraffiLearn.WebAPI.Controllers
{
    [HasPermission(Permission.AuthenticatedUser)]
    [EnableRateLimiting(RateLimitingExtensions.DefaultPolicyName)]
    [Route("api/comments")]
    [ApiController]
    public sealed class CommentsController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly ProblemDetailsFactory _problemDetailsFactory;

        public CommentsController(
            ISender sender,
            ProblemDetailsFactory problemDetailsFactory)
        {
            _sender = sender;
            _problemDetailsFactory = problemDetailsFactory;
        }

        #region Queries


        /// <include file='Documentation/CommentsControllerDocs.xml' path='doc/members/member[@name="M:GetCommentReplies"]/*'/>
        [HttpGet("{commentId:guid}/replies")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(CommentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCommentReplies(
            [FromRoute] Guid commentId)
        {
            var queryResult = await _sender.Send(new GetCommentsRepliesQuery(commentId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : _problemDetailsFactory.GetProblemDetails(queryResult);
        }


        #endregion

        #region Commands


        /// <include file='Documentation/CommentsControllerDocs.xml' path='doc/members/member[@name="M:Reply"]/*'/>
        [HttpPost("reply")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Reply(
            [FromBody] ReplyCommand replyCommand)
        {
            var commandResult = await _sender.Send(replyCommand);

            return commandResult.IsSuccess ? NoContent() : _problemDetailsFactory.GetProblemDetails(commandResult);
        }

        /// <include file='Documentation/CommentsControllerDocs.xml' path='doc/members/member[@name="M:UpdateComment"]/*'/>
        [HttpPut]
        [HasPermission(Permission.ModifyApplicationData)]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateComment(
            [FromBody] UpdateCommentCommand command)
        {
            var commandResult = await _sender.Send(command);

            return commandResult.IsSuccess ? NoContent() : _problemDetailsFactory.GetProblemDetails(commandResult);
        }

        /// <include file='Documentation/CommentsControllerDocs.xml' path='doc/members/member[@name="M:DeleteComment"]/*'/>
        [HttpDelete("{commentId:guid}")]
        [HasPermission(Permission.ModifyApplicationData)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteComment(
            [FromRoute] Guid commentId)
        {
            var commandResult = await _sender.Send(new DeleteCommentCommand(commentId));

            return commandResult.IsSuccess ? NoContent() : _problemDetailsFactory.GetProblemDetails(commandResult);
        }

        /// <include file='Documentation/CommentsControllerDocs.xml' path='doc/members/member[@name="M:LikeComment"]/*'/>
        [HttpPut("{commentId:guid}/like")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LikeComment(
            [FromRoute] Guid commentId)
        {
            var commandResult = await _sender.Send(new LikeCommentCommand(commentId));

            return commandResult.IsSuccess ? NoContent() : _problemDetailsFactory.GetProblemDetails(commandResult);
        }

        /// <include file='Documentation/CommentsControllerDocs.xml' path='doc/members/member[@name="M:DislikeComment"]/*'/>
        [HttpPut("{commentId:guid}/dislike")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DislikeComment(
            [FromRoute] Guid commentId)
        {
            var commandResult = await _sender.Send(new DislikeCommentCommand(commentId));

            return commandResult.IsSuccess ? NoContent() : _problemDetailsFactory.GetProblemDetails(commandResult);
        }

        /// <include file='Documentation/CommentsControllerDocs.xml' path='doc/members/member[@name="M:RemoveCommentLike"]/*'/>
        [HttpPut("{commentId:guid}/remove-like")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveCommentLike(
            [FromRoute] Guid commentId)
        {
            var commandResult = await _sender.Send(new RemoveCommentLikeCommand(commentId));

            return commandResult.IsSuccess ? NoContent() : _problemDetailsFactory.GetProblemDetails(commandResult);
        }

        /// <include file='Documentation/CommentsControllerDocs.xml' path='doc/members/member[@name="M:RemoveCommentDislike"]/*'/>
        [HttpPut("{commentId:guid}/remove-dislike")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveCommentDislike(
            [FromRoute] Guid commentId)
        {
            var commandResult = await _sender.Send(new RemoveCommentDislikeCommand(commentId));

            return commandResult.IsSuccess ? NoContent() : _problemDetailsFactory.GetProblemDetails(commandResult);
        }


        #endregion
    }
}
