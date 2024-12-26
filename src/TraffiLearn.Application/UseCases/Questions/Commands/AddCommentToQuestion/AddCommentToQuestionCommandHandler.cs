using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Comments;
using TraffiLearn.Domain.Comments.CommentContents;
using TraffiLearn.Domain.Questions;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Questions.Commands.AddCommentToQuestion
{
    internal sealed class AddCommentToQuestionCommandHandler : IRequestHandler<AddCommentToQuestionCommand, Result>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AddCommentToQuestionCommandHandler> _logger;

        public AddCommentToQuestionCommandHandler(
            ICommentRepository commentRepository,
            IQuestionRepository questionRepository,
            IAuthenticatedUserService authenticatedUserService,
            IUnitOfWork unitOfWork,
            ILogger<AddCommentToQuestionCommandHandler> logger)
        {
            _commentRepository = commentRepository;
            _questionRepository = questionRepository;
            _authenticatedUserService = authenticatedUserService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(
            AddCommentToQuestionCommand request,
            CancellationToken cancellationToken)
        {
            var caller = await _authenticatedUserService.GetAuthenticatedUserAsync(
                cancellationToken);

            var question = await _questionRepository.GetByIdAsync(
                new QuestionId(request.QuestionId),
                cancellationToken);

            if (question is null)
            {
                return QuestionErrors.NotFound;
            }

            var contentResult = CommentContent.Create(request.Content);

            if (contentResult.IsFailure)
            {
                return contentResult.Error;
            }

            var commentResult = Comment.Create(
                commentId: new CommentId(Guid.NewGuid()),
                contentResult.Value,
                creator: caller,
                question);

            if (commentResult.IsFailure)
            {
                return commentResult.Error;
            }

            var comment = commentResult.Value;

            question.AddComment(comment);

            await _commentRepository.InsertAsync(comment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Added the comment to the question succesfully.");

            return Result.Success();
        }
    }
}
