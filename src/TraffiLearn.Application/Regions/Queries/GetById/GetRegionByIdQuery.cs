using MediatR;
using TraffiLearn.Application.Regions.DTO;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Regions.Queries.GetById
{
    public sealed record GetRegionByIdQuery(
        Guid? RegionId) : IRequest<Result<RegionResponse>>;
}
