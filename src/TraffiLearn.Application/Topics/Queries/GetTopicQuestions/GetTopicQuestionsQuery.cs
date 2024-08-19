using MediatR;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Topics.Queries.GetTopicQuestions
{
    public sealed record GetTopicQuestionsQuery(
        Guid? TopicId) : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
