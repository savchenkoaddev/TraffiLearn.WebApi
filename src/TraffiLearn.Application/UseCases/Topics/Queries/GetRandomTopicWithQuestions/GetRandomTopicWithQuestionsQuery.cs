using MediatR;
using TraffiLearn.Application.UseCases.Topics.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Topics.Queries.GetRandomTopicWithQuestions
{
    public sealed record GetRandomTopicWithQuestionsQuery
        : IRequest<Result<TopicWithQuestionsResponse>>;
}
