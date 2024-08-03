using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Topics.Create
{
    public sealed record CreateTopicCommand(
        int? TopicNumber,
        string? Title) : IRequest<Result>;
}
