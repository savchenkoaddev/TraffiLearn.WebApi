using MediatR;

namespace TraffiLearn.Application.Commands.Topics.Delete
{
    public sealed record DeleteTopicCommand(Guid TopicId) : IRequest;
}
