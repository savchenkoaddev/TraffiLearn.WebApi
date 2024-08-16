using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.DTO.Comments;
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
        private readonly IQuestionRepository _questionRepository;
        private readonly Mapper<Question, QuestionResponse> _questionMapper;

        public GetUserDislikedQuestionsQueryHandler(
            IUserRepository userRepository,
            IQuestionRepository questionRepository,
            Mapper<Question, QuestionResponse> questionMapper)
        {
            _userRepository = userRepository;
            _questionRepository = questionRepository;
            _questionMapper = questionMapper;
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> Handle(
            GetUserDislikedQuestionsQuery request,
            CancellationToken cancellationToken)
        {
            var userId = new UserId(request.UserId.Value);

            var userExists = await _userRepository.ExistsAsync(
                userId,
                cancellationToken);

            if (!userExists)
            {
                return Result.Failure<IEnumerable<QuestionResponse>>(UserErrors.NotFound);
            }

            var dislikedQuestions = await _questionRepository.GetUserDislikedQuestionsAsync(
                userId,
                cancellationToken);

            return Result.Success(_questionMapper.Map(dislikedQuestions));
        }
    }
}
