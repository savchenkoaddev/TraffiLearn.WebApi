using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Users.GetMarkedQuestions
{
    internal sealed class GetMarkedQuestionsQueryHandler
        : IRequestHandler<GetMarkedQuestionsQuery, Result<IEnumerable<QuestionResponse>>>
    {
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IUserRepository _userRepository;
        private readonly Mapper<Question, QuestionResponse> _questionMapper;
        private readonly ILogger<GetMarkedQuestionsQueryHandler> _logger;

        public GetMarkedQuestionsQueryHandler(
            IUserContextService<Guid> userContextService,
            IUserRepository userRepository,
            Mapper<Question, QuestionResponse> questionMapper,
            ILogger<GetMarkedQuestionsQueryHandler> logger)
        {
            _userContextService = userContextService;
            _userRepository = userRepository;
            _questionMapper = questionMapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> Handle(
            GetMarkedQuestionsQuery request,
            CancellationToken cancellationToken)
        {
            var callerId = new UserId(_userContextService.FetchAuthenticatedUserId());

            var caller = await _userRepository.GetByIdAsync(
                callerId,
                cancellationToken,
                includeExpressions: [
                    user => user.MarkedQuestions
                ]);

            if (caller is null)
            {
                throw new InvalidOperationException("Authenticated user is not found.");
            }

            _logger.LogInformation(
                "Succesfully fetched user's marked questions. Questions fetched: {0}",
                caller.MarkedQuestions.Count);

            return Result.Success(_questionMapper.Map(caller.MarkedQuestions));
        }
    }
}
