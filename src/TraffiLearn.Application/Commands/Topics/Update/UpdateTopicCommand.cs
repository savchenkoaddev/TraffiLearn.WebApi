using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Topics.Update
{
    public sealed record UpdateTopicCommand(
        Guid? TopicId,
        int? TopicNumber,
        string? Title) : IRequest<Result>;
}
