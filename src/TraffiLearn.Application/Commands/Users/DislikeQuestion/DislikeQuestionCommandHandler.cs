using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Questions;

namespace TraffiLearn.Application.Commands.Users.DislikeQuestion
{
    internal sealed class DislikeQuestionCommandHandler
        : IRequestHandler<DislikeQuestionCommand, Result>
    {
        private readonly IUserManagementService _userManagementService;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DislikeQuestionCommandHandler(
            IUserManagementService userManagementService,
            IQuestionRepository questionRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _userManagementService = userManagementService;
            _questionRepository = questionRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            DislikeQuestionCommand request,
            CancellationToken cancellationToken)
        {
            var dislikerResult = await GetDisliker(
                cancellationToken);

            if (dislikerResult.IsFailure)
            {
                return dislikerResult.Error;
            }

            var questionBeingDisliked = await GetQuestionBeingDisliked(
                questionId: new QuestionId(request.QuestionId.Value),
                cancellationToken);

            if (questionBeingDisliked is null)
            {
                return UserErrors.QuestionNotFound;
            }

            var disliker = dislikerResult.Value;

            var questionDislikeResult = disliker.DislikeQuestion(questionBeingDisliked);

            if (questionDislikeResult.IsFailure)
            {
                return questionDislikeResult.Error;
            }

            var addDislikeResult = questionBeingDisliked.AddDislike(disliker);

            if (addDislikeResult.IsFailure)
            {
                return addDislikeResult.Error;
            }

            await _questionRepository.UpdateAsync(questionBeingDisliked);
            await _userRepository.UpdateAsync(disliker);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        private async Task<Result<User>> GetDisliker(
            CancellationToken cancellationToken = default)
        {
            return await _userManagementService.GetAuthenticatedUserAsync(
                cancellationToken,
                includeExpressions: [
                    user => user.LikedQuestions,
                    user => user.DislikedQuestions]);
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
