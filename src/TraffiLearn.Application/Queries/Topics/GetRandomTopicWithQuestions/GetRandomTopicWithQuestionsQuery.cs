using MediatR;
using TraffiLearn.Application.DTO.Topics;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Topics.GetRandomTopicWithQuestions
{
    public sealed record GetRandomTopicWithQuestionsQuery
        : IRequest<Result<TopicWithQuestionsResponse>>;
}
