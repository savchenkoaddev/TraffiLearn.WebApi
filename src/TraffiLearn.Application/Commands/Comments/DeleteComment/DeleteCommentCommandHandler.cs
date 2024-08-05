using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Errors.Comments;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Comments.DeleteComment
{
    internal sealed class DeleteCommentCommandHandler
        : IRequestHandler<DeleteCommentCommand, Result>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCommentCommandHandler(
            ICommentRepository commentRepository,
            IUnitOfWork unitOfWork)
        {
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            DeleteCommentCommand request, 
            CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetByIdWithAllNestedRepliesAsync(
                request.CommentId.Value,
                cancellationToken);

            if (comment is null)
            {
                return CommentErrors.NotFound;
            }

            await _commentRepository.DeleteAsync(comment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
