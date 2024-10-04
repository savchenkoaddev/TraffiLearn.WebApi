using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.ServiceCenters.DTO;
using TraffiLearn.Domain.Aggregates.ServiceCenters;
using TraffiLearn.Domain.Aggregates.ServiceCenters.Errors;
using TraffiLearn.Domain.Aggregates.ServiceCenters.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.ServiceCenters.Queries
{
    internal sealed class GetServiceCenterByIdQueryHandler
        : IRequestHandler<GetServiceCenterByIdQuery, Result<ServiceCenterResponse>>
    {
        private readonly IServiceCenterRepository _serviceCenterRepository;
        private readonly Mapper<ServiceCenter, ServiceCenterResponse> _responseMapper;

        public GetServiceCenterByIdQueryHandler(
            IServiceCenterRepository serviceCenterRepository,
            Mapper<ServiceCenter, ServiceCenterResponse> responseMapper)
        {
            _serviceCenterRepository = serviceCenterRepository;
            _responseMapper = responseMapper;
        }

        public async Task<Result<ServiceCenterResponse>> Handle(
            GetServiceCenterByIdQuery request,
            CancellationToken cancellationToken)
        {
            var serviceCenterId = new ServiceCenterId(request.ServiceCenterId.Value);

            var serviceCenter = await _serviceCenterRepository.GetByIdAsync(
                serviceCenterId,
                cancellationToken);

            if (serviceCenter is null)
            {
                return Result.Failure<ServiceCenterResponse>(
                    ServiceCenterErrors.NotFound);
            }

            return _responseMapper.Map(serviceCenter);
        }
    }
}
