using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.ServiceCenters.Commands.Delete
{
    public sealed record DeleteServiceCenterCommand(
        Guid? ServiceCenterId) : IRequest<Result>;
}
