using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.Errors;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Users.LikeQuestion
{
    internal sealed class LikeQuestionCommandHandler
        : IRequestHandler<LikeQuestionCommand, Result>
    {
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LikeQuestionCommandHandler(
            IUserContextService<Guid> userContextService,
            IQuestionRepository questionRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _userContextService = userContextService;
            _questionRepository = questionRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            LikeQuestionCommand request,
            CancellationToken cancellationToken)
        {
            var callerId = new UserId(_userContextService.FetchAuthenticatedUserId());

            var caller = await _userRepository.GetByIdAsync(
                callerId,
                cancellationToken,
                includeExpressions: [
                    user => user.LikedQuestions,
                    user => user.DislikedQuestions
                ]);

            if (caller is null)
            {
                throw new InvalidOperationException("Authenticated user is not found.");
            }

            var question = await GetQuestionBeingLiked(
                questionId: new QuestionId(request.QuestionId.Value),
                cancellationToken);

            if (question is null)
            {
                return UserErrors.QuestionNotFound;
            }

            var questionLikeResult = caller.LikeQuestion(question);

            if (questionLikeResult.IsFailure)
            {
                return questionLikeResult.Error;
            }

            var addLikeResult = question.AddLike(caller);

            if (addLikeResult.IsFailure)
            {
                return addLikeResult.Error;
            }

            await _questionRepository.UpdateAsync(question);
            await _userRepository.UpdateAsync(caller);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        private async Task<Question?> GetQuestionBeingLiked(
            QuestionId questionId,
            CancellationToken cancellationToken = default)
        {
            return await _questionRepository.GetByIdAsync(
                questionId,
                cancellationToken,
                includeExpressions: [
                    question => question.LikedByUsers,
                    question => question.DislikedByUsers
                ]);
        }
    }
}
