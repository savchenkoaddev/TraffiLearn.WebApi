using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Users.GetMarkedQuestions
{
    internal sealed class GetMarkedQuestionsQueryHandler
        : IRequestHandler<GetMarkedQuestionsQuery, Result<IEnumerable<QuestionResponse>>>
    {
        private readonly IUserManagementService _userManagementService;
        private readonly Mapper<Question, QuestionResponse> _questionMapper;
        private readonly ILogger<GetMarkedQuestionsQueryHandler> _logger;

        public GetMarkedQuestionsQueryHandler(
            IUserManagementService userManagementService,
            Mapper<Question, QuestionResponse> questionMapper,
            ILogger<GetMarkedQuestionsQueryHandler> logger)
        {
            _userManagementService = userManagementService;
            _questionMapper = questionMapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> Handle(
            GetMarkedQuestionsQuery request,
            CancellationToken cancellationToken)
        {
            var userResult = await GetCurrentUser(cancellationToken);

            if (userResult.IsFailure)
            {
                return Result.Failure<IEnumerable<QuestionResponse>>(userResult.Error);
            }

            var user = userResult.Value;

            _logger.LogInformation(
                "Succesfully fetched user's marked questions. Questions fetched: {0}",
                user.MarkedQuestions.Count);

            return Result.Success(_questionMapper.Map(user.MarkedQuestions));
        }

        private async Task<Result<User>> GetCurrentUser(
           CancellationToken cancellationToken = default)
        {
            return await _userManagementService.GetAuthenticatedUserAsync(
                cancellationToken,
                includeExpressions: [
                    user => user.MarkedQuestions
                ]);
        }
    }
}
