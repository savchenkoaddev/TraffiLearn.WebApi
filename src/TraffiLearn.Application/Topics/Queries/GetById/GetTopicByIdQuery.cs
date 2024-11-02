using MediatR;
using TraffiLearn.Application.Topics.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Topics.Queries.GetById
{
    public sealed record GetTopicByIdQuery(
        Guid? TopicId) : IRequest<Result<TopicResponse>>;
}
