﻿using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.ServiceCenters.DTO;
using TraffiLearn.Domain.ServiceCenters;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.ServiceCenters.Queries.GetById
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
            var serviceCenterId = new ServiceCenterId(request.ServiceCenterId);

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
