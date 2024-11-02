using MediatR;
using TraffiLearn.Application.Topics.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Topics.Queries.GetRandomTopicWithQuestions
{
    public sealed record GetRandomTopicWithQuestionsQuery
        : IRequest<Result<TopicWithQuestionsResponse>>;
}
