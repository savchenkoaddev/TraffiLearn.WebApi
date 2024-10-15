using MediatR;
using Microsoft.AspNetCore.Http;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Routes.Commands
{
    public sealed record CreateRouteCommand(
        Guid? ServiceCenterId,
        int? RouteNumber,
        string? Description,
        IFormFile? Image) : IRequest<Result<Guid>>;
}
