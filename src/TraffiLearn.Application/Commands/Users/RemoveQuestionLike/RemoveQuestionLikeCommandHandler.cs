using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.Errors;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Users.RemoveQuestionLike
{
    internal sealed class RemoveQuestionLikeCommandHandler
        : IRequestHandler<RemoveQuestionLikeCommand, Result>
    {
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveQuestionLikeCommandHandler(
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
            RemoveQuestionLikeCommand request,
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

            var question = await GetLikedQuestion(
                likedQuestionId: new QuestionId(request.QuestionId.Value),
                cancellationToken);

            if (question is null)
            {
                return UserErrors.QuestionNotFound;
            }

            var removeQuestionLikeResult = caller.RemoveQuestionLike(question);

            if (removeQuestionLikeResult.IsFailure)
            {
                return removeQuestionLikeResult.Error;
            }

            var removeLikeResult = question.RemoveLike(caller);

            if (removeLikeResult.IsFailure)
            {
                return removeLikeResult.Error;
            }

            await _questionRepository.UpdateAsync(question);
            await _userRepository.UpdateAsync(caller);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        private async Task<Question?> GetLikedQuestion(
            QuestionId likedQuestionId,
            CancellationToken cancellationToken = default)
        {
            return await _questionRepository.GetByIdAsync(
                likedQuestionId,
                cancellationToken,
                includeExpressions: [
                    question => question.LikedByUsersIds,
                    question => question.DislikedByUsersIds
                ]);
        }
    }
}
