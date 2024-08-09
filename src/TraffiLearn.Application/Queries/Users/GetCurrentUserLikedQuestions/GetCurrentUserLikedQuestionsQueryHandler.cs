using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Users.GetCurrentUserLikedQuestions
{
    internal sealed class GetCurrentUserLikedQuestionsQueryHandler
        : IRequestHandler<GetCurrentUserLikedQuestionsQuery,
            Result<IEnumerable<QuestionResponse>>>
    {
        private readonly IUserManagementService _userManagementService;
        private readonly Mapper<Question, QuestionResponse> _questionMapper;
        private readonly ILogger<GetCurrentUserLikedQuestionsQueryHandler> _logger;

        public GetCurrentUserLikedQuestionsQueryHandler(
            IUserManagementService userManagementService,
            Mapper<Question, QuestionResponse> questionMapper,
            ILogger<GetCurrentUserLikedQuestionsQueryHandler> logger)
        {
            _userManagementService = userManagementService;
            _questionMapper = questionMapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> Handle(
            GetCurrentUserLikedQuestionsQuery request,
            CancellationToken cancellationToken)
        {
            var userResult = await GetCurrentUser(cancellationToken);

            if (userResult.IsFailure)
            {
                return Result.Failure<IEnumerable<QuestionResponse>>(userResult.Error);
            }

            var user = userResult.Value;

            return Result.Success(_questionMapper.Map(user.LikedQuestions));
        }

        private async Task<Result<User>> GetCurrentUser(
           CancellationToken cancellationToken = default)
        {
            return await _userManagementService.GetAuthenticatedUserAsync(
                cancellationToken,
                includeExpressions: [
                    user => user.LikedQuestions
                ]);
        }
    }
}
