using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Topics.Commands.Update
{
    public sealed record UpdateTopicCommand(
        Guid? TopicId,
        int? TopicNumber,
        string? Title) : IRequest<Result>;
}
