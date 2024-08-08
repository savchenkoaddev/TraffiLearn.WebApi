using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Auth;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Commands.Users.LikeQuestion;
using TraffiLearn.Application.Errors;
using TraffiLearn.Application.Identity;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Questions;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Commands.Users.RemoveQuestionDislike
{
    internal sealed class RemoveQuestionDislikeCommandHandler
        : IRequestHandler<RemoveQuestionDislikeCommand, Result>
    {
        private readonly IAuthService<ApplicationUser> _authService;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LikeQuestionCommandHandler> _logger;

        public RemoveQuestionDislikeCommandHandler(
            IAuthService<ApplicationUser> authService,
            IQuestionRepository questionRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<LikeQuestionCommandHandler> logger)
        {
            _authService = authService;
            _questionRepository = questionRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(
            RemoveQuestionDislikeCommand request,
            CancellationToken cancellationToken)
        {
            var removerIdResult = _authService.GetAuthenticatedUserId();

            if (removerIdResult.IsFailure)
            {
                return removerIdResult.Error;
            }

            var dislikedQuestion = await GetDislikedQuestion(
                dislikedQuestionId: new QuestionId(request.QuestionId.Value),
                cancellationToken);

            if (dislikedQuestion is null)
            {
                return UserErrors.QuestionNotFound;
            }

            var remover = await GetDislikeRemover(
                removerId: new UserId(removerIdResult.Value),
                cancellationToken);

            if (remover is null)
            {
                _logger.LogCritical(InternalErrors.AuthenticatedUserNotFound.Description);

                return InternalErrors.AuthenticatedUserNotFound;
            }

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

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        private async Task<User?> GetDislikeRemover(
            UserId removerId,
            CancellationToken cancellationToken = default)
        {
            return await _userRepository.GetByIdAsync(
                removerId,
                cancellationToken,
                includeExpressions:
                    [user => user.LikedQuestions,
                        user => user.DislikedQuestions]);
        }

        private async Task<Question?> GetDislikedQuestion(
            QuestionId dislikedQuestionId,
            CancellationToken cancellationToken = default)
        {
            return await _questionRepository.GetByIdAsync(
                dislikedQuestionId,
                cancellationToken,
                includeExpressions:
                    [question => question.LikedByUsers,
                        question => question.DislikedByUsers]);
        }
    }
}
