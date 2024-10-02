using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Regions.Commands.Create
{
    public sealed record CreateRegionCommand(
        string? RegionName): IRequest<Result<Guid>>;
}
