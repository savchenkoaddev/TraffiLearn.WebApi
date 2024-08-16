using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Aggregates.Comments;
using TraffiLearn.Domain.Aggregates.Comments.ValueObjects;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.Errors;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Users.RemoveCommentDislike
{
    internal sealed class RemoveCommentDislikeCommandHandler
        : IRequestHandler<RemoveCommentDislikeCommand, Result>
    {
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IUserRepository _userRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveCommentDislikeCommandHandler(
            IUserContextService<Guid> userContextService,
            IUserRepository userRepository,
            ICommentRepository commentRepository,
            IUnitOfWork unitOfWork)
        {
            _userContextService = userContextService;
            _userRepository = userRepository;
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            RemoveCommentDislikeCommand request,
            CancellationToken cancellationToken)
        {
            var callerId = new UserId(_userContextService.FetchAuthenticatedUserId());

            var caller = await _userRepository.GetByIdAsync(
                callerId,
                cancellationToken,
                includeExpressions: [
                    user => user.LikedComments,
                    user => user.DislikedComments
                ]);

            if (caller is null)
            {
                throw new InvalidOperationException("Authenticated user is not found.");
            }

            var commentId = new CommentId(request.CommentId.Value);

            var comment = await _commentRepository.GetByIdAsync(
                commentId,
                cancellationToken,
                includeExpressions: [
                    comment => comment.LikedByUsers,
                    comment => comment.DislikedByUsers
                ]);

            if (comment is null)
            {
                return UserErrors.CommentNotFound;
            }

            var removeCommentDislikeResult = caller.RemoveCommentDislike(comment);

            if (removeCommentDislikeResult.IsFailure)
            {
                return removeCommentDislikeResult.Error;
            }

            var removeDislikeResult = comment.RemoveDislike(caller);

            if (removeDislikeResult.IsFailure)
            {
                return removeDislikeResult.Error;
            }

            await _commentRepository.UpdateAsync(comment);
            await _userRepository.UpdateAsync(caller);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
