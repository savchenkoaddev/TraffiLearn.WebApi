using MediatR;
using Microsoft.Extensions.Options;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Errors;
using TraffiLearn.Application.Options;
using TraffiLearn.Application.Services;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Comments;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Comments;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Commands.Comments.DeleteComment
{
    internal sealed class DeleteCommentCommandHandler
        : IRequestHandler<DeleteCommentCommand, Result>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserContextService
        private readonly IUserManagementService _userManagementService;
        private readonly AuthSettings _authSettings;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCommentCommandHandler(
            ICommentRepository commentRepository,
            IUserManagementService userManagementService,
            IOptions<AuthSettings> authSettings,
            IUnitOfWork unitOfWork)
        {
            _commentRepository = commentRepository;
            _userManagementService = userManagementService;
            _authSettings = authSettings.Value;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            DeleteCommentCommand request,
            CancellationToken cancellationToken)
        {
            var callerId = _userContextService.FetchAuthenticatedUserId();

            _logger.LogInformation("Succesfully fetched caller id. Caller ID: {CallerId}", callerId);

            var caller = await _userRepository.GetByIdAsync(
                new UserId(callerId),
                cancellationToken);

            if (caller is null)
            {
                _logger.LogCritical(InternalErrors.AuthorizationFailure.Description);

                return InternalErrors.AuthorizationFailure;
            }

            var userResult = await _userManagementService.GetAuthenticatedUserAsync(
                cancellationToken);

            if (userResult.IsFailure)
            {
                return userResult.Error;
            }

            var user = userResult.Value;

            if (IsNotAllowedToDeleteComments(user))
            {
                return UserErrors.NotAllowedToPerformAction;
            }

            var comment = await _commentRepository.GetByIdWithAllNestedRepliesAsync(
                commentId: new CommentId(request.CommentId.Value),
                cancellationToken);

            if (comment is null)
            {
                return CommentErrors.NotFound;
            }

            await _commentRepository.DeleteAsync(comment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        private bool IsNotAllowedToDeleteComments(User user)
        {
            return user.Role < _authSettings.MinAllowedRoleToDeleteComments;
        }
    }
}
