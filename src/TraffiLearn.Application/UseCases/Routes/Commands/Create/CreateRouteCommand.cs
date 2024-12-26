using MediatR;
using Microsoft.AspNetCore.Http;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Routes.Commands.Create
{
    public sealed record CreateRouteCommand(
        Guid ServiceCenterId,
        int RouteNumber,
        IFormFile Image,
        string? Description) : IRequest<Result<Guid>>;
}
