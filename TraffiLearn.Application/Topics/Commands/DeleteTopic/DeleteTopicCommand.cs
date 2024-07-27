using MediatR;

namespace TraffiLearn.Application.Topics.Commands.DeleteTopic
{
    public sealed record DeleteTopicCommand(
        Guid TopicId) : IRequest;
}
