using MediatR;
using TraffiLearn.Application.UseCases.Regions.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Regions.Queries.GetById
{
    public sealed record GetRegionByIdQuery(
        Guid RegionId) : IRequest<Result<RegionResponse>>;
}
