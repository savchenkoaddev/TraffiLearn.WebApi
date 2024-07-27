using MediatR;
using TraffiLearn.Application.DTO.Topics.Response;

namespace TraffiLearn.Application.Topics.Queries.GetAllSorted
{
    public sealed record GetAllSortedTopicsQuery : IRequest<IEnumerable<TopicResponse>>;
}
