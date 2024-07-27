using MediatR;
using TraffiLearn.Application.DTO.Topics.Response;

namespace TraffiLearn.Application.Topics.Queries.GetById
{
    public sealed record GetTopicByIdQuery(
        Guid TopicId) : IRequest<TopicResponse>;
}
