using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Aggregates.Comments;
using TraffiLearn.Domain.Aggregates.Comments.Errors;
using TraffiLearn.Domain.Aggregates.Comments.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Comments.Commands.UpdateComment
{
    internal sealed class UpdateCommentCommandHandler
        : IRequestHandler<UpdateCommentCommand, Result>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCommentCommandHandler(
            ICommentRepository commentRepository,
            IUnitOfWork unitOfWork)
        {
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            UpdateCommentCommand request,
            CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetByIdAsync(
                new CommentId(request.CommentId.Value),
                cancellationToken);

            if (comment is null)
            {
                return CommentErrors.NotFound;
            }

            var newContentResult = CommentContent.Create(request.Content);

            if (newContentResult.IsFailure)
            {
                return newContentResult.Error;
            }

            var newContent = newContentResult.Value;

            var updateResult = comment.Update(newContent);

            if (updateResult.IsFailure)
            {
                return updateResult.Error;
            }

            await _commentRepository.UpdateAsync(comment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
