using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Regions;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Regions.Commands.Create
{
    internal sealed class CreateRegionCommandHandler
        : IRequestHandler<CreateRegionCommand, Result<Guid>>
    {
        private readonly IRegionRepository _regionRepository;
        private readonly Mapper<CreateRegionCommand, Result<Region>> _requestMapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateRegionCommandHandler(
            IRegionRepository regionRepository,
            Mapper<CreateRegionCommand, Result<Region>> requestMapper,
            IUnitOfWork unitOfWork)
        {
            _regionRepository = regionRepository;
            _requestMapper = requestMapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            CreateRegionCommand request,
            CancellationToken cancellationToken)
        {
            var mappingResult = _requestMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return Result.Failure<Guid>(mappingResult.Error);
            }

            var region = mappingResult.Value;

            await _regionRepository.InsertAsync(
                region: region,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(region.Id.Value);
        }
    }
}
