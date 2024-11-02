using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Domain.Questions;
using TraffiLearn.Domain.Users;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Users.Queries.GetMarkedQuestions
{
    internal sealed class GetMarkedQuestionsQueryHandler
        : IRequestHandler<GetMarkedQuestionsQuery, Result<IEnumerable<QuestionResponse>>>
    {
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IUserRepository _userRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly Mapper<Question, QuestionResponse> _questionMapper;
        private readonly ILogger<GetMarkedQuestionsQueryHandler> _logger;

        public GetMarkedQuestionsQueryHandler(
            IUserContextService<Guid> userContextService,
            IUserRepository userRepository,
            IQuestionRepository questionRepository,
            Mapper<Question, QuestionResponse> questionMapper,
            ILogger<GetMarkedQuestionsQueryHandler> logger)
        {
            _userContextService = userContextService;
            _userRepository = userRepository;
            _questionRepository = questionRepository;
            _questionMapper = questionMapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> Handle(
            GetMarkedQuestionsQuery request,
            CancellationToken cancellationToken)
        {
            var userId = new UserId(_userContextService.GetAuthenticatedUserId());

            var userExists = await _userRepository.ExistsAsync(
                userId,
                cancellationToken);

            if (!userExists)
            {
                throw new InvalidOperationException("Authenticated user is not found.");
            }

            var markedQuestions = await _questionRepository.GetUserMarkedQuestionsAsync(
                userId,
                cancellationToken);

            return Result.Success(_questionMapper.Map(markedQuestions));
        }
    }
}
