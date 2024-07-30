using MediatR;
using TraffiLearn.Application.DTO.Topics;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Topics.GetAllSortedByNumber
{
    public sealed record GetAllSortedTopicsByNumberQuery : IRequest<Result<IEnumerable<TopicResponse>>>;
}
