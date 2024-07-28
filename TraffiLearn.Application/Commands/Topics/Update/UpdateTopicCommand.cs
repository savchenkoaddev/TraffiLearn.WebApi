using MediatR;

namespace TraffiLearn.Application.Commands.Topics.Update
{
    public sealed record UpdateTopicCommand(
        Guid TopicId,
        int TopicNumber,
        string Title) : IRequest;
}
