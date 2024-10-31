using MediatR;
using TraffiLearn.Application.Regions.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Regions.Queries.GetAll
{
    public sealed record GetAllRegionsQuery :
        IRequest<Result<IEnumerable<RegionResponse>>>;
}
