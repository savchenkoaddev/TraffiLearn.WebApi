using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Questions;

namespace TraffiLearn.Application.Commands.Users.LikeQuestion
{
    internal sealed class LikeQuestionCommandHandler
        : IRequestHandler<LikeQuestionCommand, Result>
    {
        private readonly IUserManagementService _userManagementService;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LikeQuestionCommandHandler(
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
            LikeQuestionCommand request,
            CancellationToken cancellationToken)
        {
            var likerResult = await GetLiker(
                cancellationToken);

            if (likerResult.IsFailure)
            {
                return likerResult.Error;
            }

            var question = await GetQuestionBeingLiked(
                questionId: new QuestionId(request.QuestionId.Value),
                cancellationToken);

            if (question is null)
            {
                return UserErrors.QuestionNotFound;
            }

            var liker = likerResult.Value;

            var questionLikeResult = liker.LikeQuestion(question);

            if (questionLikeResult.IsFailure)
            {
                return questionLikeResult.Error;
            }

            var addLikeResult = question.AddLike(liker);

            if (addLikeResult.IsFailure)
            {
                return addLikeResult.Error;
            }

            await _questionRepository.UpdateAsync(question);
            await _userRepository.UpdateAsync(liker);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        private async Task<Result<User>> GetLiker(
            CancellationToken cancellationToken = default)
        {
            return await _userManagementService.GetAuthenticatedUserAsync(
                cancellationToken,
                includeExpressions: [
                    user => user.LikedQuestions,
                    user => user.DislikedQuestions
                ]);
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
