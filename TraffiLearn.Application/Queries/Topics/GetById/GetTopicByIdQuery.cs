using MediatR;
using TraffiLearn.Application.DTO.Topics;

namespace TraffiLearn.Application.Queries.Topics.GetById
{
    public sealed record GetTopicByIdQuery(Guid? TopicId) : IRequest<TopicResponse>;
}
