using MediatR;
using TraffiLearn.Application.UseCases.Regions.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Regions.Queries.GetAll
{
    public sealed record GetAllRegionsQuery :
        IRequest<Result<IEnumerable<RegionResponse>>>;
}
