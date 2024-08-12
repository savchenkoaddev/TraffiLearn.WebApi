using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

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
