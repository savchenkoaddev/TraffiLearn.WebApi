using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Topics.Commands.Delete
{
    public sealed record DeleteTopicCommand(
        Guid? TopicId) : IRequest<Result>;
}
