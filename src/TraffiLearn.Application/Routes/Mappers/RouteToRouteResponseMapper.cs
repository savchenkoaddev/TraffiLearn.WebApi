using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Routes.DTO;
using TraffiLearn.Domain.Routes;

namespace TraffiLearn.Application.Routes.Mappers
{
    internal sealed class RouteToRouteResponseMapper
        : Mapper<Route, RouteResponse>
    {
        public override RouteResponse Map(Route source)
        {
            string? description = null;

            if (source.Description is not null)
            {
                description = source.Description.Value;
            }

            return new RouteResponse(
                Id: source.Id.Value,
                RouteNumber: source.RouteNumber.Value,
                Description: description,
                ImageUri: source.ImageUri.Value);
        }
    }
}
