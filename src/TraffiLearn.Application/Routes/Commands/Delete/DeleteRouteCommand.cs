using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Routes.Commands.Delete
{
    public sealed record DeleteRouteCommand(
        Guid? RouteId) : IRequest<Result>;
}
