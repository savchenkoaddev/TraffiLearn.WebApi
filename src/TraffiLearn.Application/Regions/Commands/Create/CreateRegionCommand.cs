using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Regions.Commands.Create
{
    public sealed record CreateRegionCommand(
        string? RegionName) : IRequest<Result<Guid>>;
}
