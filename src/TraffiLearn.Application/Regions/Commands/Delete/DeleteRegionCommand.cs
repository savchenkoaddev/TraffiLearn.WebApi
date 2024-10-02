using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Regions.Commands.Delete
{
    public sealed record DeleteRegionCommand(
        Guid? RegionId) : IRequest<Result>;
}
