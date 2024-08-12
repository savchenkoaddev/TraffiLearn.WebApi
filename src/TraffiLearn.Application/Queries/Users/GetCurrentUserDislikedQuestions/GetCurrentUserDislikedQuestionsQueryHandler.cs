using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Queries.Users.GetCurrentUserDislikedQuestions
{
    internal sealed class GetCurrentUserDislikedQuestionsQueryHandler
        : IRequestHandler<GetCurrentUserDislikedQuestionsQuery,
            Result<IEnumerable<QuestionResponse>>>
    {
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IUserRepository _userRepository;
        private readonly Mapper<Question, QuestionResponse> _questionMapper;
        private readonly ILogger<GetCurrentUserDislikedQuestionsQueryHandler> _logger;

        public GetCurrentUserDislikedQuestionsQueryHandler(
            IUserContextService<Guid> userContextService,
            IUserRepository userRepository,
            Mapper<Question, QuestionResponse> questionMapper,
            ILogger<GetCurrentUserDislikedQuestionsQueryHandler> logger)
        {
            _userContextService = userContextService;
            _userRepository = userRepository;
            _questionMapper = questionMapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> Handle(
            GetCurrentUserDislikedQuestionsQuery request,
            CancellationToken cancellationToken)
        {
            var userId = new UserId(_userContextService.FetchAuthenticatedUserId());

            var user = await _userRepository.GetByIdAsync(
                userId,
                cancellationToken,
                includeExpressions: [
                    user => user.DislikedQuestions
                ]);

            if (user is null)
            {
                throw new InvalidOperationException("Authenticated user is not found.");
            }

            return Result.Success(_questionMapper.Map(user.DislikedQuestions));
        }
    }
}
