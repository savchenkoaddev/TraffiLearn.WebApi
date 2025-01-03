﻿using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Regions;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Regions.Commands.Delete
{
    internal sealed class DeleteRegionCommandHandler
        : IRequestHandler<DeleteRegionCommand, Result>
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRegionCommandHandler(
            IRegionRepository regionRepository,
            IUnitOfWork unitOfWork)
        {
            _regionRepository = regionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            DeleteRegionCommand request,
            CancellationToken cancellationToken)
        {
            var regionId = new RegionId(request.RegionId);

            var region = await _regionRepository.GetByIdAsync(
                regionId,
                cancellationToken);

            if (region is null)
            {
                return RegionErrors.NotFound;
            }

            await _regionRepository.DeleteAsync(region);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
