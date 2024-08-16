using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Aggregates.Comments;
using TraffiLearn.Domain.Aggregates.Comments.Errors;
using TraffiLearn.Domain.Aggregates.Comments.ValueObjects;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Comments.Reply
{
    internal sealed class ReplyCommandHandler : IRequestHandler<ReplyCommand, Result>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReplyCommandHandler> _logger;

        public ReplyCommandHandler(
            ICommentRepository commentRepository,
            IUserRepository userRepository,
            IAuthenticatedUserService authenticatedUserService,
            IUnitOfWork unitOfWork,
            ILogger<ReplyCommandHandler> logger)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _authenticatedUserService = authenticatedUserService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(
            ReplyCommand request,
            CancellationToken cancellationToken)
        {
            var caller = await _authenticatedUserService
                .GetAuthenticatedUserAsync(cancellationToken);

            var comment = await _commentRepository.GetByIdAsync(
                commentId: new CommentId(request.CommentId.Value),
                cancellationToken,
                includeExpressions: [
                    comment => comment.QuestionId
                ]);

            if (comment is null)
            {
                return CommentErrors.NotFound;
            }

            var commentContentResult = CommentContent.Create(request.Content);

            if (commentContentResult.IsFailure)
            {
                return commentContentResult.Error;
            }

            CommentId replyCommentId = new(Guid.NewGuid());

            var replyCommentResult = Comment.Create(
                commentId: replyCommentId,
                content: commentContentResult.Value,
                creator: caller,
                question: comment.QuestionId);

            if (replyCommentResult.IsFailure)
            {
                return replyCommentResult.Error;
            }

            var replyComment = replyCommentResult.Value;

            var replyResult = comment.Reply(replyComment);

            if (replyResult.IsFailure)
            {
                return replyResult.Error;
            }

            var userAddCommentResult = caller.AddComment(replyComment);

            if (userAddCommentResult.IsFailure)
            {
                return userAddCommentResult.Error;
            }

            var questionAddCommentResult = comment.QuestionId.AddComment(replyComment);

            if (questionAddCommentResult.IsFailure)
            {
                return questionAddCommentResult.Error;
            }

            await _commentRepository.AddAsync(
                replyComment,
                cancellationToken);

            await _userRepository.UpdateAsync(caller);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Replied to the comment succesfully.");

            return Result.Success();
        }
    }
}
