using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Topics.Commands.Delete
{
    public sealed record DeleteTopicCommand(
        Guid? TopicId) : IRequest<Result>;
}
