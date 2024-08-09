using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Comments;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Comments;

namespace TraffiLearn.Application.Commands.Comments.Reply
{
    internal sealed class ReplyCommandHandler : IRequestHandler<ReplyCommand, Result>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserManagementService _userManagementService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReplyCommandHandler> _logger;

        public ReplyCommandHandler(
            ICommentRepository commentRepository,
            IUserRepository userRepository,
            IUserManagementService userManagementService,
            IUnitOfWork unitOfWork,
            ILogger<ReplyCommandHandler> logger)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _userManagementService = userManagementService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(
            ReplyCommand request,
            CancellationToken cancellationToken)
        {
            var userResult = await _userManagementService.GetAuthenticatedUserAsync(
    cancellationToken);

            if (userResult.IsFailure)
            {
                return userResult.Error;
            }

            var comment = await _commentRepository.GetByIdAsync(
                commentId: new CommentId(request.CommentId.Value),
                cancellationToken,
                includeExpressions: comment => comment.Question);

            if (comment is null)
            {
                return CommentErrors.NotFound;
            }

            var user = userResult.Value;

            var commentContentResult = CommentContent.Create(request.Content);

            if (commentContentResult.IsFailure)
            {
                return commentContentResult.Error;
            }

            CommentId replyCommentId = new(Guid.NewGuid());

            var replyCommentResult = Comment.Create(
                commentId: replyCommentId,
                content: commentContentResult.Value,
                leftBy: user,
                question: comment.Question);

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

            var userAddCommentResult = user.AddComment(replyComment);

            if (userAddCommentResult.IsFailure)
            {
                return userAddCommentResult.Error;
            }

            var questionAddCommentResult = comment.Question.AddComment(replyComment);

            if (questionAddCommentResult.IsFailure)
            {
                return questionAddCommentResult.Error;
            }

            await _commentRepository.AddAsync(
                replyComment,
                cancellationToken);

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Replied to the comment succesfully.");

            return Result.Success();
        }
    }
}
