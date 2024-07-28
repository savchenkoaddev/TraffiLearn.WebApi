using MediatR;

namespace TraffiLearn.Application.Commands.Topics.Create
{
    public sealed record CreateTopicCommand(
        int TopicNumber,
        string Title) : IRequest;
}
