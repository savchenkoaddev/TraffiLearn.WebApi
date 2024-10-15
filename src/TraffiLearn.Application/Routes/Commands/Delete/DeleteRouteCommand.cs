using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Routes.Commands.Delete
{
    public sealed record DeleteRouteCommand(
        Guid? RouteId) : IRequest<Result>;
}
