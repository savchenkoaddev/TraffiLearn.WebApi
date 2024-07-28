using MediatR;
using TraffiLearn.Application.DTO.Topics;

namespace TraffiLearn.Application.Queries.Topics.GetAllSortedByNumber
{
    public sealed record GetAllSortedTopicsByNumberQuery : IRequest<IEnumerable<TopicResponse>>;
}
