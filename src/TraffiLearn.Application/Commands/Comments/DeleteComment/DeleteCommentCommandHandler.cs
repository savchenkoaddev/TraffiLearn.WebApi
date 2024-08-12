using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Errors.Comments;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Comments;

namespace TraffiLearn.Application.Commands.Comments.DeleteComment
{
    internal sealed class DeleteCommentCommandHandler
        : IRequestHandler<DeleteCommentCommand, Result>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteCommentCommandHandler> _logger;

        public DeleteCommentCommandHandler(
            ICommentRepository commentRepository,
            IUnitOfWork unitOfWork,
            ILogger<DeleteCommentCommandHandler> logger)
        {
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(
            DeleteCommentCommand request,
            CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetByIdWithAllNestedRepliesAsync(
                commentId: new CommentId(request.CommentId.Value),
                cancellationToken);

            if (comment is null)
            {
                return CommentErrors.NotFound;
            }

            await _commentRepository.DeleteAsync(comment);

            var changed = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (changed > 0)
            {
                _logger.LogInformation("Successfully deleted comment with ID {CommentId}.", request.CommentId.Value);
            }
            else
            {
                _logger.LogWarning("No changes were made during deletion of comment with ID {CommentId}.", request.CommentId.Value);
            }

            return Result.Success();
        }
    }
}
