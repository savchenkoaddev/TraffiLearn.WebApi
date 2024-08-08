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

namespace TraffiLearn.Application.Commands.Users.RemoveQuestionLike
{
    internal sealed class RemoveQuestionLikeCommandHandler
        : IRequestHandler<RemoveQuestionLikeCommand, Result>
    {
        private readonly IAuthService<ApplicationUser> _authService;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LikeQuestionCommandHandler> _logger;

        public RemoveQuestionLikeCommandHandler(
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
            RemoveQuestionLikeCommand request,
            CancellationToken cancellationToken)
        {
            var removerIdResult = _authService.GetAuthenticatedUserId();

            if (removerIdResult.IsFailure)
            {
                return removerIdResult.Error;
            }

            var question = await GetLikedQuestion(
                likedQuestionId: new QuestionId(request.QuestionId.Value));

            if (question is null)
            {
                return UserErrors.QuestionNotFound;
            }

            var remover = await GetLikeRemover(
                removerId: new UserId(removerIdResult.Value),
                cancellationToken);

            if (remover is null)
            {
                _logger.LogCritical(InternalErrors.AuthenticatedUserNotFound.Description);

                return InternalErrors.AuthenticatedUserNotFound;
            }

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

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        private async Task<User?> GetLikeRemover(
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

        private async Task<Question?> GetLikedQuestion(
            QuestionId likedQuestionId,
            CancellationToken cancellationToken = default)
        {
            return await _questionRepository.GetByIdAsync(
                likedQuestionId,
                cancellationToken,
                includeExpressions:
                    [question => question.LikedByUsers,
                        question => question.DislikedByUsers]);
        }
    }
}
