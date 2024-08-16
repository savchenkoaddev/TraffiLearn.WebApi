using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.Errors;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Users.RemoveQuestionDislike
{
    internal sealed class RemoveQuestionDislikeCommandHandler
        : IRequestHandler<RemoveQuestionDislikeCommand, Result>
    {
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveQuestionDislikeCommandHandler(
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
            RemoveQuestionDislikeCommand request,
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

            var dislikedQuestion = await GetDislikedQuestion(
                dislikedQuestionId: new QuestionId(request.QuestionId.Value),
                cancellationToken);

            if (dislikedQuestion is null)
            {
                return UserErrors.QuestionNotFound;
            }

            var removeQuestionDislikeResult = caller.RemoveQuestionDislike(dislikedQuestion);

            if (removeQuestionDislikeResult.IsFailure)
            {
                return removeQuestionDislikeResult.Error;
            }

            var removeDislikeResult = dislikedQuestion.RemoveDislike(caller);

            if (removeDislikeResult.IsFailure)
            {
                return removeDislikeResult.Error;
            }

            await _questionRepository.UpdateAsync(dislikedQuestion);
            await _userRepository.UpdateAsync(caller);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        private async Task<Question?> GetDislikedQuestion(
            QuestionId dislikedQuestionId,
            CancellationToken cancellationToken = default)
        {
            return await _questionRepository.GetByIdAsync(
                dislikedQuestionId,
                cancellationToken,
                includeExpressions: [
                    question => question.LikedByUsers,
                    question => question.DislikedByUsers
                ]);
        }
    }
}
