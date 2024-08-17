using MediatR;
using TraffiLearn.Application.Topics.DTO;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Topics.Queries.GetRandomTopicWithQuestions
{
    public sealed record GetRandomTopicWithQuestionsQuery
        : IRequest<Result<TopicWithQuestionsResponse>>;
}
