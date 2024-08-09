using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Questions;

namespace TraffiLearn.Application.Commands.Users.RemoveQuestionLike
{
    internal sealed class RemoveQuestionLikeCommandHandler
        : IRequestHandler<RemoveQuestionLikeCommand, Result>
    {
        private readonly IUserManagementService _userManagementService;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveQuestionLikeCommandHandler(
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
            RemoveQuestionLikeCommand request,
            CancellationToken cancellationToken)
        {
            var removerResult = await GetLikeRemover(cancellationToken);

            if (removerResult.IsFailure)
            {
                return removerResult.Error;
            }

            var question = await GetLikedQuestion(
                likedQuestionId: new QuestionId(request.QuestionId.Value),
                cancellationToken);

            if (question is null)
            {
                return UserErrors.QuestionNotFound;
            }

            var remover = removerResult.Value;

            var removeQuestionLikeResult = remover.RemoveQuestionLike(question);

            if (removeQuestionLikeResult.IsFailure)
            {
                return removeQuestionLikeResult.Error;
            }

            var removeLikeResult = question.RemoveLike(remover);

            if (removeLikeResult.IsFailure)
            {
                return removeLikeResult.Error;
            }

            await _questionRepository.UpdateAsync(question);
            await _userRepository.UpdateAsync(remover);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        private async Task<Result<User>> GetLikeRemover(
            CancellationToken cancellationToken = default)
        {
            return await _userManagementService.GetAuthenticatedUserAsync(
                cancellationToken,
                includeExpressions: [
                    user => user.LikedQuestions,
                    user => user.DislikedQuestions
                ]);
        }

        private async Task<Question?> GetLikedQuestion(
            QuestionId likedQuestionId,
            CancellationToken cancellationToken = default)
        {
            return await _questionRepository.GetByIdAsync(
                likedQuestionId,
                cancellationToken,
                includeExpressions: [
                    question => question.LikedByUsers,
                    question => question.DislikedByUsers
                ]);
        }
    }
}
