﻿using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.ServiceCenters.DTO;
using TraffiLearn.Domain.Aggregates.Regions;
using TraffiLearn.Domain.Aggregates.Regions.Errors;
using TraffiLearn.Domain.Aggregates.Regions.ValueObjects;
using TraffiLearn.Domain.Aggregates.ServiceCenters;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.ServiceCenters.Queries.GetByRegionId
{
    internal sealed class GetServiceCentersByRegionIdQueryHandler
        : IRequestHandler<GetServiceCentersByRegionIdQuery,
            Result<IEnumerable<ServiceCenterResponse>>>
    {
        private readonly IServiceCenterRepository _serviceCenterRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly Mapper<ServiceCenter, ServiceCenterResponse> _responseMapper;

        public GetServiceCentersByRegionIdQueryHandler(
            IServiceCenterRepository serviceCenterRepository,
            IRegionRepository regionRepository,
            Mapper<ServiceCenter, ServiceCenterResponse> responseMapper)
        {
            _serviceCenterRepository = serviceCenterRepository;
            _regionRepository = regionRepository;
            _responseMapper = responseMapper;
        }

        public async Task<Result<IEnumerable<ServiceCenterResponse>>> Handle(
            GetServiceCentersByRegionIdQuery request,
            CancellationToken cancellationToken)
        {
            var regionId = new RegionId(request.RegionId.Value);

            var regionExists = await _regionRepository.ExistsAsync(
                regionId: regionId,
                cancellationToken);

            if (!regionExists)
            {
                return Result.Failure<IEnumerable<ServiceCenterResponse>>(
                    RegionErrors.NotFound);
            }

            var serviceCenters = await _serviceCenterRepository
                .GetServiceCentersByRegionId(
                    regionId, cancellationToken);

            return Result.Success(_responseMapper.Map(serviceCenters));
        }
    }
}
