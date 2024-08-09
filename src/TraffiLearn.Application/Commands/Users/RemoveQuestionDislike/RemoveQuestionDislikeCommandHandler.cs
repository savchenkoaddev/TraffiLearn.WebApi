using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Commands.Users.LikeQuestion;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Questions;

namespace TraffiLearn.Application.Commands.Users.RemoveQuestionDislike
{
    internal sealed class RemoveQuestionDislikeCommandHandler
        : IRequestHandler<RemoveQuestionDislikeCommand, Result>
    {
        private readonly IUserManagementService _userManagementService;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveQuestionDislikeCommandHandler(
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
            RemoveQuestionDislikeCommand request,
            CancellationToken cancellationToken)
        {
            var removerResult = await GetDislikeRemover(cancellationToken);

            if (removerResult.IsFailure)
            {
                return removerResult.Error;
            }

            var dislikedQuestion = await GetDislikedQuestion(
                dislikedQuestionId: new QuestionId(request.QuestionId.Value),
                cancellationToken);

            if (dislikedQuestion is null)
            {
                return UserErrors.QuestionNotFound;
            }

            var remover = removerResult.Value;

            var removeQuestionDislikeResult = remover.RemoveQuestionDislike(dislikedQuestion);

            if (removeQuestionDislikeResult.IsFailure)
            {
                return removeQuestionDislikeResult.Error;
            }

            var removeDislikeResult = dislikedQuestion.RemoveDislike(remover);

            if (removeDislikeResult.IsFailure)
            {
                return removeDislikeResult.Error;
            }

            await _questionRepository.UpdateAsync(dislikedQuestion);
            await _userRepository.UpdateAsync(remover);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        private async Task<Result<User>> GetDislikeRemover(
            CancellationToken cancellationToken = default)
        {
            return await _userManagementService.GetAuthenticatedUserAsync(
                cancellationToken,
                includeExpressions: [
                    user => user.LikedQuestions,
                    user => user.DislikedQuestions
                ]);
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
