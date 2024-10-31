using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Users.Queries.GetUserLikedQuestions
{
    internal sealed class GetUserLikedQuestionsQueryHandler
        : IRequestHandler<GetUserLikedQuestionsQuery, Result<IEnumerable<QuestionResponse>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly Mapper<Question, QuestionResponse> _questionMapper;

        public GetUserLikedQuestionsQueryHandler(
            IUserRepository userRepository,
            IQuestionRepository questionRepository,
            Mapper<Question, QuestionResponse> questionMapper)
        {
            _userRepository = userRepository;
            _questionRepository = questionRepository;
            _questionMapper = questionMapper;
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> Handle(
            GetUserLikedQuestionsQuery request,
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

            var likedQuestions = await _questionRepository.GetUserLikedQuestionsAsync(
                userId,
                cancellationToken);

            return Result.Success(_questionMapper.Map(likedQuestions));
        }
    }
}
