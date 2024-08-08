using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Auth;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Application.Errors;
using TraffiLearn.Application.Identity;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Queries.Users.GetCurrentUserLikedQuestions
{
    internal sealed class GetCurrentUserLikedQuestionsQueryHandler
        : IRequestHandler<GetCurrentUserLikedQuestionsQuery,
            Result<IEnumerable<QuestionResponse>>>
    {
        private readonly IAuthService<ApplicationUser> _authService;
        private readonly IUserRepository _userRepository;
        private readonly Mapper<Question, QuestionResponse> _questionMapper;
        private readonly ILogger<GetCurrentUserLikedQuestionsQueryHandler> _logger;

        public GetCurrentUserLikedQuestionsQueryHandler(
            IAuthService<ApplicationUser> authService,
            IUserRepository userRepository,
            Mapper<Question, QuestionResponse> questionMapper,
            ILogger<GetCurrentUserLikedQuestionsQueryHandler> logger)
        {
            _authService = authService;
            _userRepository = userRepository;
            _questionMapper = questionMapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> Handle(
            GetCurrentUserLikedQuestionsQuery request,
            CancellationToken cancellationToken)
        {
            var userIdResult = _authService.GetAuthenticatedUserId();

            if (userIdResult.IsFailure)
            {
                return Result.Failure<IEnumerable<QuestionResponse>>(userIdResult.Error);
            }

            var user = await _userRepository.GetByIdAsync(
                userId: new UserId(userIdResult.Value),
                cancellationToken,
                includeExpressions: user => user.LikedQuestions);

            if (user is null)
            {
                _logger.LogCritical(InternalErrors.AuthenticatedUserNotFound.Description);

                return Result.Failure<IEnumerable<QuestionResponse>>(InternalErrors.AuthenticatedUserNotFound);
            }

            return Result.Success(_questionMapper.Map(user.LikedQuestions));
        }
    }
}
