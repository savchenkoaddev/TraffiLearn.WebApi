using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Routes.DTO;
using TraffiLearn.Domain.Aggregates.Routes;
using TraffiLearn.Domain.Aggregates.ServiceCenters;
using TraffiLearn.Domain.Aggregates.ServiceCenters.Errors;
using TraffiLearn.Domain.Aggregates.ServiceCenters.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Routes.Queries.GetByServiceCenterId
{
    internal sealed class GetRoutesByServiceCenterIdQueryHandler
        : IRequestHandler<GetRoutesByServiceCenterIdQuery,
            Result<IEnumerable<RouteResponse>>>
    {
        private readonly IRouteRepository _routeRepository;
        private readonly IServiceCenterRepository _serviceCenterRepository;
        private readonly Mapper<Route, RouteResponse> _responseMapper;

        public GetRoutesByServiceCenterIdQueryHandler(
            IRouteRepository routeRepository,
            IServiceCenterRepository serviceCenterRepository,
            Mapper<Route, RouteResponse> responseMapper)
        {
            _routeRepository = routeRepository;
            _serviceCenterRepository = serviceCenterRepository;
            _responseMapper = responseMapper;
        }

        public async Task<Result<IEnumerable<RouteResponse>>> Handle(
            GetRoutesByServiceCenterIdQuery request,
            CancellationToken cancellationToken)
        {
            var serviceCenterId = new ServiceCenterId(request.ServiceCenterId.Value);

            var serviceCenterExists = await _serviceCenterRepository.ExistsAsync(
                serviceCenterId, 
                cancellationToken);

            if (!serviceCenterExists)
            {
                return Result.Failure<IEnumerable<RouteResponse>>(ServiceCenterErrors.NotFound);
            }

            var routes = await _routeRepository.GetRoutesByServiceCenterId(
                serviceCenterId,
                cancellationToken);

            return Result.Success(_responseMapper.Map(routes));
        }
    }
}
