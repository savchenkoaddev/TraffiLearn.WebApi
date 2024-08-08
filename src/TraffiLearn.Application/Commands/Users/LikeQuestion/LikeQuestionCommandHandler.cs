using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using TraffiLearn.Application.Abstractions.Auth;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Errors;
using TraffiLearn.Application.Identity;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Questions;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Commands.Users.LikeQuestion
{
    internal sealed class LikeQuestionCommandHandler
        : IRequestHandler<LikeQuestionCommand, Result>
    {
        private readonly IAuthService<ApplicationUser> _authService;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LikeQuestionCommandHandler> _logger;

        public LikeQuestionCommandHandler(
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
            LikeQuestionCommand request,
            CancellationToken cancellationToken)
        {
            var likerIdResult = _authService.GetAuthenticatedUserId();

            if (likerIdResult.IsFailure)
            {
                return likerIdResult.Error;
            }

            var question = await GetQuestionBeingLiked(
                questionId: new QuestionId(request.QuestionId.Value),
                cancellationToken);

            if (question is null)
            {
                return UserErrors.QuestionNotFound;
            }

            var liker = await GetLiker(
                likerId: new UserId(likerIdResult.Value),
                cancellationToken);

            if (liker is null)
            {
                _logger.LogCritical(InternalErrors.AuthenticatedUserNotFound.Description);

                return InternalErrors.AuthenticatedUserNotFound;
            }

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

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        private async Task<Question?> GetQuestionBeingLiked(
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

        private async Task<User?> GetLiker(
            UserId likerId,
            CancellationToken cancellationToken = default)
        {
            return await _userRepository.GetByIdAsync(
                likerId,
                cancellationToken,
                includeExpressions:
                    [user => user.LikedQuestions,
                        user => user.DislikedQuestions]);
        }
    }
}
