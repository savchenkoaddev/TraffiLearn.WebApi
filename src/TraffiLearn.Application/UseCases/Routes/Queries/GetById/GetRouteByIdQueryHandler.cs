using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.Routes.DTO;
using TraffiLearn.Domain.Routes;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Routes.Queries.GetById
{
    internal sealed class GetRouteByIdQueryHandler
        : IRequestHandler<GetRouteByIdQuery, Result<RouteResponse>>
    {
        private readonly IRouteRepository _routeRepository;
        private readonly Mapper<Route, RouteResponse> _routeMapper;

        public GetRouteByIdQueryHandler(
            IRouteRepository routeRepository,
            Mapper<Route, RouteResponse> routeMapper)
        {
            _routeRepository = routeRepository;
            _routeMapper = routeMapper;
        }

        public async Task<Result<RouteResponse>> Handle(
            GetRouteByIdQuery request,
            CancellationToken cancellationToken)
        {
            var route = await _routeRepository.GetByIdAsync(
                routeId: new RouteId(request.RouteId),
                cancellationToken);

            if (route is null)
            {
                return Result.Failure<RouteResponse>(
                    RouteErrors.NotFound);
            }

            return _routeMapper.Map(route);
        }
    }
}
