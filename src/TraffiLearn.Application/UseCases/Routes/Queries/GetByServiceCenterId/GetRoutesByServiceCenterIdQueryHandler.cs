using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.Routes.DTO;
using TraffiLearn.Domain.Routes;
using TraffiLearn.Domain.ServiceCenters;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Routes.Queries.GetByServiceCenterId
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