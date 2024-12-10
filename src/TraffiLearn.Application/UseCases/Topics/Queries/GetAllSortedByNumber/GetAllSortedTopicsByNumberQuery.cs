using MediatR;
using TraffiLearn.Application.UseCases.Topics.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Topics.Queries.GetAllSortedByNumber
{
    public sealed record GetAllSortedTopicsByNumberQuery : IRequest<Result<IEnumerable<TopicResponse>>>;
}
