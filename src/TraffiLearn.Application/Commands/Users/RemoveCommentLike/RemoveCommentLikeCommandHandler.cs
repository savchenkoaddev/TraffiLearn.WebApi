using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Comments;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Commands.Users.RemoveCommentLike
{
    internal sealed class RemoveCommentLikeCommandHandler
        : IRequestHandler<RemoveCommentLikeCommand, Result>
    {
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IUserRepository _userRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveCommentLikeCommandHandler(
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
            RemoveCommentLikeCommand request,
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

            var removeCommentLikeResult = caller.RemoveCommentLike(comment);

            if (removeCommentLikeResult.IsFailure)
            {
                return removeCommentLikeResult.Error;
            }

            var removeLikeResult = comment.RemoveLike(caller);

            if (removeLikeResult.IsFailure)
            {
                return removeLikeResult.Error;
            }

            await _commentRepository.UpdateAsync(comment);
            await _userRepository.UpdateAsync(caller);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
