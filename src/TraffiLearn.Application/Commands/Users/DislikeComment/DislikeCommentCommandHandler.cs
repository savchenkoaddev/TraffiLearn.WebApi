using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Aggregates.Comments;
using TraffiLearn.Domain.Aggregates.Comments.ValueObjects;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.Errors;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Users.DislikeComment
{
    internal sealed class DislikeCommentCommandHandler
        : IRequestHandler<DislikeCommentCommand, Result>
    {
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IUserRepository _userRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DislikeCommentCommandHandler(
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
            DislikeCommentCommand request,
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

            var dislikeCommentResult = caller.DislikeComment(comment);

            if (dislikeCommentResult.IsFailure)
            {
                return dislikeCommentResult.Error;
            }

            var addDislikeResult = comment.AddDislike(caller);

            if (addDislikeResult.IsFailure)
            {
                return addDislikeResult.Error;
            }

            await _commentRepository.UpdateAsync(comment);
            await _userRepository.UpdateAsync(caller);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
