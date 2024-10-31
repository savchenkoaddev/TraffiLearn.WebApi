using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Domain.Routes;
using TraffiLearn.Domain.ServiceCenters;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Routes.Commands.Create
{
    internal sealed class CreateRouteCommandHandler
        : IRequestHandler<CreateRouteCommand, Result<Guid>>
    {
        private readonly IRouteRepository _routeRepository;
        private readonly IServiceCenterRepository _serviceCenterRepository;
        private readonly IImageService _imageService;
        private readonly Mapper<CreateRouteCommand, Result<Route>> _requestMapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateRouteCommandHandler(
            IRouteRepository routeRepository,
            IServiceCenterRepository serviceCenterRepository,
            IImageService imageService,
            Mapper<CreateRouteCommand, Result<Route>> requestMapper,
            IUnitOfWork unitOfWork)
        {
            _routeRepository = routeRepository;
            _serviceCenterRepository = serviceCenterRepository;
            _imageService = imageService;
            _requestMapper = requestMapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            CreateRouteCommand request,
            CancellationToken cancellationToken)
        {
            var mappingResult = _requestMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return Result.Failure<Guid>(mappingResult.Error);
            }

            var serviceCenterId = new ServiceCenterId(request.ServiceCenterId.Value);

            var serviceCenter = await _serviceCenterRepository.GetByIdAsync(
                serviceCenterId,
                cancellationToken);

            if (serviceCenter is null)
            {
                return Result.Failure<Guid>(ServiceCenterErrors.NotFound);
            }

            var route = mappingResult.Value;

            var addResult = serviceCenter.AddRoute(route);
            var setScResult = route.SetServiceCenter(serviceCenter);

            if (addResult.IsFailure || setScResult.IsFailure)
            {
                throw new InvalidOperationException(
                    "Failed to add route to service center or vice versa due to some internal issues. " +
                    "This could be because of incorrect entity ID generation.");
            }

            await _serviceCenterRepository.UpdateAsync(serviceCenter);

            var routeImageUri = await _imageService.UploadImageAsync(
                image: request.Image,
                cancellationToken);

            try
            {
                route.SetImageUri(routeImageUri);

                await _routeRepository.InsertAsync(route, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception)
            {
                await _imageService.DeleteAsync(
                    imageUri: routeImageUri, cancellationToken);

                throw;
            }

            return Result.Success(route.Id.Value);
        }
    }
}
