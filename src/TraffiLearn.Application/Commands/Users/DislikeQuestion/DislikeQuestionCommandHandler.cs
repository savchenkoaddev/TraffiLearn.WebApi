using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Questions;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Commands.Users.DislikeQuestion
{
    internal sealed class DislikeQuestionCommandHandler
        : IRequestHandler<DislikeQuestionCommand, Result>
    {
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DislikeQuestionCommandHandler(
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
            DislikeQuestionCommand request,
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
                throw new InvalidOperationException("Authenticated user not found.");
            }

            var questionBeingDisliked = await GetQuestionBeingDisliked(
                questionId: new QuestionId(request.QuestionId.Value),
                cancellationToken);

            if (questionBeingDisliked is null)
            {
                return UserErrors.QuestionNotFound;
            }

            var questionDislikeResult = caller.DislikeQuestion(questionBeingDisliked);

            if (questionDislikeResult.IsFailure)
            {
                return questionDislikeResult.Error;
            }

            var addDislikeResult = questionBeingDisliked.AddDislike(caller);

            if (addDislikeResult.IsFailure)
            {
                return addDislikeResult.Error;
            }

            await _questionRepository.UpdateAsync(questionBeingDisliked);
            await _userRepository.UpdateAsync(caller);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        private async Task<Question?> GetQuestionBeingDisliked(
            QuestionId questionId,
            CancellationToken cancellationToken = default)
        {
            return await _questionRepository.GetByIdAsync(
                questionId,
                cancellationToken,
                includeExpressions: [
                    question => question.LikedByUsers,
                    question => question.DislikedByUsers]);
        }
    }
}
