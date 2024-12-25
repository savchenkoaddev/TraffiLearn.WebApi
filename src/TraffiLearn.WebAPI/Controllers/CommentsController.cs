using MediatR;
using Microsoft.AspNetCore.Mvc;
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
using TraffiLearn.WebAPI.Extensions;
using TraffiLearn.WebAPI.Swagger;

namespace TraffiLearn.WebAPI.Controllers
{
    [HasPermission(Permission.AuthenticatedUser)]
    [Route("api/comments")]
    [ApiController]
    public sealed class CommentsController : ControllerBase
    {
        private readonly ISender _sender;

        public CommentsController(ISender sender)
        {
            _sender = sender;
        }

        #region Queries


        /// <summary>
        /// Gets all replies of a comment.
        /// </summary>
        /// <remarks>
        /// **The request must include an ID of a comment.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `CommentId` : Must be a valid GUID of a comment.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.
        /// </remarks>
        /// <param name="commentId">**The ID of a comment, which replies are being retrieved**</param>
        /// <response code="200">Successfully retrieved comment replies with the provided comment ID. Returns comment replies.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="404">***Not found.*** No comment exists with the provided ID.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpGet("{commentId:guid}/replies")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(CommentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCommentReplies(
            [FromRoute] Guid commentId)
        {
            var queryResult = await _sender.Send(new GetCommentsRepliesQuery(commentId));

            return queryResult.IsSuccess ? Ok(queryResult.Value) : queryResult.ToProblemDetails();
        }


        #endregion

        #region Commands


        /// <summary>
        /// Adds a reply to a comment.
        /// </summary>
        /// <remarks>
        /// **Reply** is basically a comment that is a response to another comment.<br /><br />
        /// **If added reply**, comment is going to contain the reply.<br /><br />
        /// ***Parameters:***<br /><br />
        /// `CommentId` : ID of the comment to reply to. Must be a valid GUID.<br /><br />
        /// `Content` : Content of the reply. Must not be empty. Must be less than 500 characters long.<br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token.
        /// </remarks>
        /// <param name="replyCommand">The reply command.</param>
        /// <response code="204">Successfully replied to a comment.</response>
        /// <response code="400">***Bad request.*** The provided data is invalid or missing.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="404">***Not found.*** Comment with the id is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
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

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <summary>
        /// Updates an existing comment.
        /// </summary>
        /// <remarks>
        /// ***Parameters:***<br /><br />
        /// `CommentId` : ID of the comment to be updated. Must be a valid GUID.<br /><br />
        /// `Content` : Content of the comment. Must not be empty. Must be less than 500 characters long.<br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="command"></param>
        /// <response code="204">Successfully updated an existing comment.</response>
        /// <response code="400">***Bad request.*** The provided data is invalid or missing.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** Comment with the id is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
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

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <summary>
        /// Deletes a comment.
        /// </summary>
        /// <remarks>
        /// **The request must include the ID of the comment.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `CommentId` : Must be a valid GUID representing ID of the comment.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="commentId">**The ID of the comment to be deleted.**</param>
        /// <response code="204">Successfully deleted the comment.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** Comment with the provided id is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpDelete("{commentId:guid}")]
        [HasPermission(Permission.ModifyApplicationData)]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteComment(
            [FromRoute] Guid commentId)
        {
            var commandResult = await _sender.Send(new DeleteCommentCommand(commentId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <summary>
        /// Adds a like to a comment.
        /// </summary>
        /// <remarks>
        /// **If comment is liked**, comment can not be liked.<br /><br />
        /// **If comment is disliked**, comment should be undisliked first and then liked.<br /><br />
        /// **The request must include the ID of the comment.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `CommentId` : ID of the comment to like. Must be a valid GUID.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="commentId"></param>
        /// <response code="204">Successfully liked the comment.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** Comment with the provided id is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpPut("{commentId:guid}/like")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LikeComment(
            [FromRoute] Guid commentId)
        {
            var commandResult = await _sender.Send(new LikeCommentCommand(commentId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <summary>
        /// Adds a dislike to a comment.
        /// </summary>
        /// <remarks>
        /// **If comment is disliked**, comment can not be liked.<br /><br />
        /// **If comment is liked**, comment should be unliked first and then disliked.<br /><br />
        /// **The request must include the ID of the comment.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `CommentId` : ID of the comment to dislike. Must be a valid GUID.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="commentId"></param>
        /// <response code="204">Successfully disliked the comment.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** Comment with the provided id is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpPut("{commentId:guid}/dislike")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DislikeComment(
            [FromRoute] Guid commentId)
        {
            var commandResult = await _sender.Send(new DislikeCommentCommand(commentId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <summary>
        /// Removes a like from a comment.
        /// </summary>
        /// <remarks>
        /// **If comment is not liked**, comment can not be unliked.<br /><br />
        /// **The request must include the ID of the comment.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `CommentId` : ID of the comment to remove like from. Must be a valid GUID.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="commentId"></param>
        /// <response code="204">Successfully removed like from the comment.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** Comment with the provided id is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpPut("{commentId:guid}/remove-like")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveCommentLike(
            [FromRoute] Guid commentId)
        {
            var commandResult = await _sender.Send(new RemoveCommentLikeCommand(commentId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }

        /// <summary>
        /// Removes a dislike from a comment.
        /// </summary>
        /// <remarks>
        /// **If comment is not disliked**, comment can not be undisliked.<br /><br />
        /// **The request must include the ID of the comment.**<br /><br /><br />
        /// ***Route parameters:***<br /><br />
        /// `CommentId` : ID of the comment to remove dislike from. Must be a valid GUID.<br /><br /><br />
        /// **Authentication Required:**<br />
        /// The user must be authenticated using a JWT token. Only users with the `Owner` or `Admin` role can perform this action.<br /><br />
        /// </remarks>
        /// <param name="commentId"></param>
        /// <response code="204">Successfully removed dislike from the comment.</response>
        /// <response code="401">***Unauthorized.*** The user is not authenticated.</response>
        /// <response code="403">***Forbidden***. The user is not authorized to perform this action.</response>
        /// <response code="404">***Not found.*** Comment with the provided id is not found.</response>
        /// <response code="500">***Internal Server Error.*** An unexpected error occurred during the process.</response>
        [HttpPut("{commentId:guid}/remove-dislike")]
        [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.ProblemJson)]
        [ProducesResponseType(typeof(ClientErrorResponseExample), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ServerErrorResponseExample), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveCommentDislike(
            [FromRoute] Guid commentId)
        {
            var commandResult = await _sender.Send(new RemoveCommentDislikeCommand(commentId));

            return commandResult.IsSuccess ? NoContent() : commandResult.ToProblemDetails();
        }


        #endregion
    }
}
