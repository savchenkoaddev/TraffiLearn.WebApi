using MediatR;
using Microsoft.AspNetCore.Http;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Routes.Commands.Create
{
    public sealed record CreateRouteCommand(
        Guid? ServiceCenterId,
        int? RouteNumber,
        string? Description,
        IFormFile? Image) : IRequest<Result<Guid>>;
}
