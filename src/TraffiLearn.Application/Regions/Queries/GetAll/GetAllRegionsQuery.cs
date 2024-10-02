using MediatR;
using TraffiLearn.Application.Regions.DTO;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Regions.Queries.GetAll
{
    public sealed record GetAllRegionsQuery :
        IRequest<Result<IEnumerable<RegionResponse>>>;
}
