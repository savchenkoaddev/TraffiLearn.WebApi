using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Comments;
using TraffiLearn.Domain.Users;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Commands.LikeComment
{
    internal sealed class LikeCommentCommandHandler
        : IRequestHandler<LikeCommentCommand, Result>
    {
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IUserRepository _userRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LikeCommentCommandHandler(
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
            LikeCommentCommand request,
            CancellationToken cancellationToken)
        {
            var callerId = new UserId(_userContextService.GetAuthenticatedUserId());

            var caller = await _userRepository.GetByIdWithLikedAndDislikedCommentsAsync(
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

            var likeCommentResult = caller.LikeComment(comment);

            if (likeCommentResult.IsFailure)
            {
                return likeCommentResult.Error;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
