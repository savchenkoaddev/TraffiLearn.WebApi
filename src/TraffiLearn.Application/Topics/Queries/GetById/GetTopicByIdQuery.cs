using MediatR;
using TraffiLearn.Application.Topics.DTO;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Topics.Queries.GetById
{
    public sealed record GetTopicByIdQuery(
        Guid? TopicId) : IRequest<Result<TopicResponse>>;
}
