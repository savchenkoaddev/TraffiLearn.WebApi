using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Routes.Commands.Create;
using TraffiLearn.Domain.Routes;
using TraffiLearn.Domain.Routes.RouteDescriptions;
using TraffiLearn.Domain.Routes.RouteNumbers;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Routes.Mappers
{
    internal sealed class CreateRouteCommandToEntityMapper
        : Mapper<CreateRouteCommand, Result<Route>>
    {
        public override Result<Route> Map(CreateRouteCommand source)
        {
            var numberResult = RouteNumber.Create(source.RouteNumber.Value);

            if (numberResult.IsFailure)
            {
                return Result.Failure<Route>(numberResult.Error);
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

            var routeId = new RouteId(Guid.NewGuid());

            var routeResult = Route.Create(
                routeId,
                routeNumber: numberResult.Value,
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
