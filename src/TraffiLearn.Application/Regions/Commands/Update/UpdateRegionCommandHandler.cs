using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Aggregates.Regions;
using TraffiLearn.Domain.Aggregates.Regions.Errors;
using TraffiLearn.Domain.Aggregates.Regions.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Regions.Commands.Update
{
    internal sealed class UpdateRegionCommandHandler
        : IRequestHandler<UpdateRegionCommand, Result>
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRegionCommandHandler(
            IRegionRepository regionRepository, 
            IUnitOfWork unitOfWork)
        {
            _regionRepository = regionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            UpdateRegionCommand request, 
            CancellationToken cancellationToken)
        {
            var regionId = new RegionId(request.RegionId.Value);

            var region = await _regionRepository.GetByIdAsync(
                regionId, 
                cancellationToken);

            if (region is null)
            {
                return RegionErrors.NotFound;
            }

            var newRegionNameResult = RegionName.Create(request.RegionName);

            if (newRegionNameResult.IsFailure)
            {
                return newRegionNameResult.Error;
            }

            var updateResult = region.Update(newRegionNameResult.Value);

            if (updateResult.IsFailure)
            {
                return updateResult.Error;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
