using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Topics.Commands.Create
{
    public sealed record CreateTopicCommand(
        int? TopicNumber,
        string? Title) : IRequest<Result>;
}
