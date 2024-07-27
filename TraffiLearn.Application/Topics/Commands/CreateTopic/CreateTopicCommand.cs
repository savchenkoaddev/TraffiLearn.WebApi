using MediatR;
using TraffiLearn.Application.DTO.Topics.Request;

namespace TraffiLearn.Application.Topics.Commands.CreateTopic
{
    public sealed record CreateTopicCommand(
        TopicRequest RequestObject) : IRequest;
}
