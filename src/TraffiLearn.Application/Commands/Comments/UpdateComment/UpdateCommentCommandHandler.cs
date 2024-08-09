using MediatR;
using Microsoft.EntityFrameworkCore;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Identity;
using TraffiLearn.Domain.Entities;
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
        private readonly IUserManagementService _userManagementService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCommentCommandHandler(
            ICommentRepository commentRepository,
            IUserManagementService userManagementService,
            IUnitOfWork unitOfWork)
        {
            _commentRepository = commentRepository;
            _userManagementService = userManagementService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            UpdateCommentCommand request,
            CancellationToken cancellationToken)
        {
            var userResult = await _userManagementService.GetAuthenticatedUserAsync(
                cancellationToken);

            if (userResult.IsFailure)
            {
                return userResult.Error;
            }

            var comment = await GetComment(
                new CommentId(request.CommentId.Value), 
                cancellationToken);

            if (comment is null)
            {
                return CommentErrors.NotFound;
            }

            var user = userResult.Value;

            if (IsNotAllowedAllowedToUpdate(comment, user))
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

        private async Task<Comment?> GetComment(
            CommentId commentId, 
            CancellationToken cancellationToken = default)
        {
            return await _commentRepository.GetByIdAsync(
                            commentId,
                            cancellationToken,
                            includeExpressions: comment => comment.User);
        }

        private bool IsNotAllowedAllowedToUpdate(
            Comment comment,
            User user)
        {
            return comment.User.Id.Value != user.Id.Value;
        }
    }
}
