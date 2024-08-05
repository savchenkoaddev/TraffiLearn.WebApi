using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Auth;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Identity;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Comments;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Comments;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Commands.Comments.Reply
{
    internal sealed class ReplyCommandHandler : IRequestHandler<ReplyCommand, Result>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAuthService<ApplicationUser> _authService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReplyCommandHandler> _logger;

        public ReplyCommandHandler(
            ICommentRepository commentRepository,
            IUserRepository userRepository,
            IAuthService<ApplicationUser> authService,
            IUnitOfWork unitOfWork,
            ILogger<ReplyCommandHandler> logger)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _authService = authService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(
            ReplyCommand request,
            CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetByIdAsync(
                request.CommentId.Value,
                cancellationToken,
                includeExpressions: comment => comment.Question);

            if (comment is null)
            {
                return CommentErrors.NotFound;
            }

            Result<Email> emailResult = _authService.GetAuthenticatedUserEmail();

            if (emailResult.IsFailure)
            {
                return emailResult.Error;
            }

            var email = emailResult.Value;

            var user = await _userRepository.GetByEmailAsync(
                email,
                cancellationToken);

            if (user is null)
            {
                _logger.LogError("User with the context provided email hasn't been found. Seems there's some issue with the data consistency.");

                return Error.InternalFailure();
            }

            var commentContentResult = CommentContent.Create(request.Content);

            if (commentContentResult.IsFailure)
            {
                return commentContentResult.Error;
            }

            var replyCommentId = Guid.NewGuid();

            var replyCommentResult = Comment.Create(
                id: replyCommentId,
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

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Replied to the comment succesfully.");

            return Result.Success();
        }
    }
}
