using MediatR;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Topics.GetTopicQuestions
{
    public sealed record GetTopicQuestionsQuery(
        Guid? TopicId) : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
