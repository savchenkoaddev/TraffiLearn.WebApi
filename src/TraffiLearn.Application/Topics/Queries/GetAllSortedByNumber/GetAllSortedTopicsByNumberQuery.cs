using MediatR;
using TraffiLearn.Application.Topics.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Topics.Queries.GetAllSortedByNumber
{
    public sealed record GetAllSortedTopicsByNumberQuery : IRequest<Result<IEnumerable<TopicResponse>>>;
}
