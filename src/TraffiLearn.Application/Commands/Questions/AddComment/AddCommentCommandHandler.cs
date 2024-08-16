using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Aggregates.Comments;
using TraffiLearn.Domain.Aggregates.Comments.ValueObjects;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.Errors;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Questions.AddComment
{
    internal sealed class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, Result>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AddCommentCommandHandler> _logger;

        public AddCommentCommandHandler(
            ICommentRepository commentRepository,
            IUserRepository userRepository,
            IQuestionRepository questionRepository,
            IUserContextService<Guid> userContextService,
            IUnitOfWork unitOfWork,
            ILogger<AddCommentCommandHandler> logger)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _questionRepository = questionRepository;
            _userContextService = userContextService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(
            AddCommentCommand request,
            CancellationToken cancellationToken)
        {
            var callerId = new UserId(_userContextService.FetchAuthenticatedUserId());

            var caller = await _userRepository.GetByIdAsync(
                callerId,
                cancellationToken,
                includeExpressions: [
                    user => user.Comments
                ]);

            if (caller is null)
            {
                throw new InvalidOperationException("Authenticated user not found.");
            }

            var question = await _questionRepository.GetByIdAsync(
                questionId: new QuestionId(request.QuestionId.Value),
                cancellationToken,
                includeExpressions: question => question.Comments);

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

            var userAddCommentResult = caller.AddComment(comment);

            if (userAddCommentResult.IsFailure)
            {
                return userAddCommentResult.Error;
            }

            var questionAddCommentResult = question.AddComment(comment);

            if (questionAddCommentResult.IsFailure)
            {
                return questionAddCommentResult.Error;
            }

            await _commentRepository.AddAsync(
                comment,
                cancellationToken);
            await _questionRepository.UpdateAsync(question);
            await _userRepository.UpdateAsync(caller);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Added the comment to the question succesfully.");

            return Result.Success();
        }
    }
}
