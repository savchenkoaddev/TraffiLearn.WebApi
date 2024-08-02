using MediatR;
using TraffiLearn.Application.Abstractions.Auth;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Identity;
using TraffiLearn.Domain.Errors.Comments;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Comments;

namespace TraffiLearn.Application.Commands.Comments.UpdateComment
{
    internal sealed class UpdateCommentCommandHandler
        : IRequestHandler<UpdateCommentCommand, Result>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IAuthService<ApplicationUser> _authService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCommentCommandHandler(
            ICommentRepository commentRepository,
            IAuthService<ApplicationUser> authService,
            IUnitOfWork unitOfWork)
        {
            _commentRepository = commentRepository;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            UpdateCommentCommand request,
            CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetByIdAsync(
                request.CommentId.Value,
                includeExpression: x => x.User);

            if (comment is null)
            {
                return CommentErrors.NotFound;
            }

            var idResult = _authService.GetAuthenticatedUserId();

            if (idResult.IsFailure)
            {
                return idResult.Error;
            }

            var id = idResult.Value;

            if (comment.User.Id != id)
            {
                return CommentErrors.NotAllowedToModify;
            }

            var newContentResult = CommentContent.Create(request.Content);

            if (newContentResult.IsFailure)
            {
                return newContentResult.Error;
            }

            var newContent = newContentResult.Value;

            comment.UpdateContent(newContent);

            await _commentRepository.UpdateAsync(comment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
