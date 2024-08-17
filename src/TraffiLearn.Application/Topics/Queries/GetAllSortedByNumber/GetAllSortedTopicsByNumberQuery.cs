using MediatR;
using TraffiLearn.Application.Topics.DTO;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Topics.Queries.GetAllSortedByNumber
{
    public sealed record GetAllSortedTopicsByNumberQuery : IRequest<Result<IEnumerable<TopicResponse>>>;
}
