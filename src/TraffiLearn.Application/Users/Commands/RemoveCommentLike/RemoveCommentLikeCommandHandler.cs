using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Aggregates.Comments;
using TraffiLearn.Domain.Aggregates.Comments.Errors;
using TraffiLearn.Domain.Aggregates.Comments.ValueObjects;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Users.Commands.RemoveCommentLike
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

            var caller = await _userRepository.GetByIdWithLikedAndDislikedCommentsIdsAsync(
                callerId,
                cancellationToken);

            if (caller is null)
            {
                throw new InvalidOperationException("Authenticated user is not found.");
            }

            var comment = await _commentRepository.GetByIdAsync(
                commentId: new CommentId(request.CommentId.Value),
                cancellationToken);

            if (comment is null)
            {
                return CommentErrors.NotFound;
            }

            var removeLikeResult = caller.RemoveCommentLike(comment.Id);

            if (removeLikeResult.IsFailure)
            {
                return removeLikeResult.Error;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
