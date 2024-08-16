using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Users.GetCurrentUserLikedQuestions
{
    internal sealed class GetCurrentUserLikedQuestionsQueryHandler
        : IRequestHandler<GetCurrentUserLikedQuestionsQuery,
            Result<IEnumerable<QuestionResponse>>>
    {
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IUserRepository _userRepository;
        private readonly Mapper<Question, QuestionResponse> _questionMapper;
        private readonly ILogger<GetCurrentUserLikedQuestionsQueryHandler> _logger;

        public GetCurrentUserLikedQuestionsQueryHandler(
            IUserContextService<Guid> userContextService,
            IUserRepository userRepository,
            Mapper<Question, QuestionResponse> questionMapper,
            ILogger<GetCurrentUserLikedQuestionsQueryHandler> logger)
        {
            _userContextService = userContextService;
            _userRepository = userRepository;
            _questionMapper = questionMapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> Handle(
            GetCurrentUserLikedQuestionsQuery request,
            CancellationToken cancellationToken)
        {
            var userId = new UserId(_userContextService.FetchAuthenticatedUserId());

            var user = await _userRepository.GetByIdAsync(
                userId,
                cancellationToken,
                includeExpressions: [
                    user => user.LikedQuestions
                ]);

            if (user is null)
            {
                throw new InvalidOperationException("Authenticated user is not found.");
            }

            return Result.Success(_questionMapper.Map(user.LikedQuestions));
        }
    }
}
