using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
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

namespace TraffiLearn.Application.Commands.Users.DislikeQuestion
{
    internal sealed class DislikeQuestionCommandHandler
        : IRequestHandler<DislikeQuestionCommand, Result>
    {
        private readonly IAuthService<ApplicationUser> _authService;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LikeQuestionCommandHandler> _logger;

        public DislikeQuestionCommandHandler(
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
            DislikeQuestionCommand request,
            CancellationToken cancellationToken)
        {
            var dislikerIdResult = _authService.GetAuthenticatedUserId();

            if (dislikerIdResult.IsFailure)
            {
                return dislikerIdResult.Error;
            }

            var questionBeingDisliked = await GetQuestionBeingDisliked(
                questionId: new QuestionId(request.QuestionId.Value),
                cancellationToken);

            if (questionBeingDisliked is null)
            {
                return UserErrors.QuestionNotFound;
            }

            var disliker = await GetDisliker(
                dislikerId: new UserId(dislikerIdResult.Value),
                cancellationToken);

            if (disliker is null)
            {
                _logger.LogCritical(InternalErrors.AuthenticatedUserNotFound.Description);

                return InternalErrors.AuthenticatedUserNotFound;
            }

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
                includeExpressions:
                    [question => question.LikedByUsers,
                        question => question.DislikedByUsers]);
        }

        private async Task<User?> GetDisliker(
            UserId dislikerId,
            CancellationToken cancellationToken = default)
        {
            return await _userRepository.GetByIdAsync(
                userId: dislikerId,
                cancellationToken,
                includeExpressions:
                    [user => user.LikedQuestions,
                        user => user.DislikedQuestions]);
        }
    }
}
