using MediatR;
using TraffiLearn.Application.UseCases.Topics.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Topics.Queries.GetById
{
    public sealed record GetTopicByIdQuery(
        Guid TopicId) : IRequest<Result<TopicResponse>>;
}
