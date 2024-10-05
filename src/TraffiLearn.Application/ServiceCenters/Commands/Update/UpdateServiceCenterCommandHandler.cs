using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Aggregates.Regions;
using TraffiLearn.Domain.Aggregates.Regions.Errors;
using TraffiLearn.Domain.Aggregates.Regions.ValueObjects;
using TraffiLearn.Domain.Aggregates.ServiceCenters;
using TraffiLearn.Domain.Aggregates.ServiceCenters.Errors;
using TraffiLearn.Domain.Aggregates.ServiceCenters.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.ServiceCenters.Commands.Update
{
    internal sealed class UpdateServiceCenterCommandHandler
        : IRequestHandler<UpdateServiceCenterCommand, Result>
    {
        private readonly IServiceCenterRepository _serviceCenterRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly Mapper<UpdateServiceCenterCommand, Result<ServiceCenter>> _requestMapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateServiceCenterCommandHandler(
            IServiceCenterRepository serviceCenterRepository, 
            IRegionRepository regionRepository,
            Mapper<UpdateServiceCenterCommand, Result<ServiceCenter>> requestMapper,
            IUnitOfWork unitOfWork)
        {
            _serviceCenterRepository = serviceCenterRepository;
            _regionRepository = regionRepository;
            _requestMapper = requestMapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            UpdateServiceCenterCommand request, 
            CancellationToken cancellationToken)
        {
            var mappingResult = _requestMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return mappingResult.Error;
            }

            var serviceCenterId = new ServiceCenterId(
                request.ServiceCenterId.Value);

            var serviceCenter = await _serviceCenterRepository
                .GetByIdAsync(
                    serviceCenterId,
                    cancellationToken);

            if (serviceCenter is null)
            {
                return ServiceCenterErrors.NotFound;
            }

            var regionId = new RegionId(request.RegionId.Value);

            var region = await _regionRepository.GetByIdAsync(
                regionId, cancellationToken);

            if (region is null)
            {
                return RegionErrors.NotFound;
            }

            var newServiceCenter = mappingResult.Value;

            var updateResult = serviceCenter.Update(
                address: newServiceCenter.Address,
                number: newServiceCenter.Number);

            if (updateResult.IsFailure)
            {
                return updateResult.Error;
            }

            await _serviceCenterRepository.UpdateAsync(serviceCenter);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
