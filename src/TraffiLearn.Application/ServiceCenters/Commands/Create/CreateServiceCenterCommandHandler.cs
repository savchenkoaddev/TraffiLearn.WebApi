using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Aggregates.Regions;
using TraffiLearn.Domain.Aggregates.Regions.Errors;
using TraffiLearn.Domain.Aggregates.Regions.ValueObjects;
using TraffiLearn.Domain.Aggregates.ServiceCenters;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.ServiceCenters.Commands.Create
{
    internal sealed class CreateServiceCenterCommandHandler
        : IRequestHandler<CreateServiceCenterCommand, Result<Guid>>
    {
        private readonly IServiceCenterRepository _scRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly Mapper<CreateServiceCenterCommand, Result<ServiceCenter>> _requestMapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateServiceCenterCommandHandler(
            IServiceCenterRepository scRepository,
            IRegionRepository regionRepository,
            Mapper<CreateServiceCenterCommand, Result<ServiceCenter>> requestMapper,
            IUnitOfWork unitOfWork)
        {
            _scRepository = scRepository;
            _regionRepository = regionRepository;
            _requestMapper = requestMapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            CreateServiceCenterCommand request,
            CancellationToken cancellationToken)
        {
            var mappingResult = _requestMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return Result.Failure<Guid>(mappingResult.Error);
            }

            var regionId = new RegionId(request.RegionId.Value);

            var region = await _regionRepository.GetByIdAsync(
                regionId,
                cancellationToken);

            if (region is null)
            {
                return Result.Failure<Guid>(RegionErrors.NotFound);
            }

            var serviceCenter = mappingResult.Value;

            var result = serviceCenter.SetRegion(region);

            if (result.IsFailure)
            {
                return Result.Failure<Guid>(result.Error);
            }

            await _scRepository.InsertAsync(
                serviceCenter, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(serviceCenter.Id.Value);
        }
    }
}
