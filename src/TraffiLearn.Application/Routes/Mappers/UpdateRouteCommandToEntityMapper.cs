using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Routes.Commands.Update;
using TraffiLearn.Domain.Aggregates.Routes;
using TraffiLearn.Domain.Aggregates.Routes.RouteDescriptions;
using TraffiLearn.Domain.Aggregates.Routes.RouteNumbers;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Routes.Mappers
{
    internal sealed class UpdateRouteCommandToEntityMapper
        : Mapper<UpdateRouteCommand, Result<Route>>
    {
        public override Result<Route> Map(UpdateRouteCommand source)
        {
            var routeNumberResult = RouteNumber.Create(source.RouteNumber.Value);

            if (routeNumberResult.IsFailure)
            {
                return Result.Failure<Route>(routeNumberResult.Error);
            }

            RouteDescription? description = default;

            if (source.Description is not null)
            {
                var descriptionResult = RouteDescription.Create(source.Description);

                if (descriptionResult.IsFailure)
                {
                    return Result.Failure<Route>(descriptionResult.Error);
                }

                description = descriptionResult.Value;
            }

            var routeId = new RouteId(source.RouteId.Value);

            var routeResult = Route.Create(
                routeId,
                routeNumber: routeNumberResult.Value,
                routeDescription: description,
                imageUri: null);

            if (routeResult.IsFailure)
            {
                return Result.Failure<Route>(routeResult.Error);
            }

            return routeResult.Value;
        }
    }
}
