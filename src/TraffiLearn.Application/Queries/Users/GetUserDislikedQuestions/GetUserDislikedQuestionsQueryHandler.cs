using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.Errors;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Users.GetUserDislikedQuestions
{
    internal sealed class GetUserDislikedQuestionsQueryHandler
        : IRequestHandler<GetUserDislikedQuestionsQuery, Result<IEnumerable<QuestionResponse>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly Mapper<Question, QuestionResponse> _questionMapper;

        public GetUserDislikedQuestionsQueryHandler(
            IUserRepository userRepository,
            Mapper<Question, QuestionResponse> questionMapper)
        {
            _userRepository = userRepository;
            _questionMapper = questionMapper;
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> Handle(
            GetUserDislikedQuestionsQuery request,
            CancellationToken cancellationToken)
        {
            UserId userId = new(request.UserId.Value);

            var user = await _userRepository.GetByIdAsync(
                userId: new UserId(request.UserId.Value),
                cancellationToken,
                includeExpressions: user => user.DislikedQuestions);

            if (user is null)
            {
                return Result.Failure<IEnumerable<QuestionResponse>>(UserErrors.NotFound);
            }

            return Result.Success(_questionMapper.Map(user.DislikedQuestions));
        }
    }
}
