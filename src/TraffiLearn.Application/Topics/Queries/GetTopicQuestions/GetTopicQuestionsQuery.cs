using MediatR;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Topics.Queries.GetTopicQuestions
{
    public sealed record GetTopicQuestionsQuery(
        Guid? TopicId) : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
