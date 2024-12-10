using MediatR;
using Microsoft.AspNetCore.Http;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Domain.Routes;
using TraffiLearn.Domain.ServiceCenters;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Routes.Commands.Update
{
    internal sealed class UpdateRouteCommandHandler
        : IRequestHandler<UpdateRouteCommand, Result>
    {
        private readonly IRouteRepository _routeRepository;
        private readonly IServiceCenterRepository _serviceCenterRepository;
        private readonly IImageService _imageService;
        private readonly Mapper<UpdateRouteCommand, Result<Route>> _requestMapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRouteCommandHandler(
            IRouteRepository routeRepository,
            IServiceCenterRepository serviceCenterRepository,
            IImageService imageService,
            Mapper<UpdateRouteCommand, Result<Route>> requestMapper,
            IUnitOfWork unitOfWork)
        {
            _routeRepository = routeRepository;
            _serviceCenterRepository = serviceCenterRepository;
            _imageService = imageService;
            _requestMapper = requestMapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            UpdateRouteCommand request,
            CancellationToken cancellationToken)
        {
            var mappingResult = _requestMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return mappingResult.Error;
            }

            var routeId = new RouteId(request.RouteId.Value);

            var route = await _routeRepository.GetByIdWithServiceCenterAsync(
                routeId, cancellationToken);

            if (route is null)
            {
                return RouteErrors.NotFound;
            }

            var serviceCenterId = new ServiceCenterId(request.ServiceCenterId.Value);

            var serviceCenter = await _serviceCenterRepository.GetByIdAsync(
                serviceCenterId, cancellationToken);

            if (serviceCenter is null)
            {
                return ServiceCenterErrors.NotFound;
            }

            var newRoute = mappingResult.Value;

            var updateResult = UpdateExistingRoute(
                route: route,
                newRoute: newRoute);

            if (updateResult.IsFailure)
            {
                return updateResult.Error;
            }

            if (route.ServiceCenter != serviceCenter)
            {
                await UpdateRouteServiceCenter(route, serviceCenter);
            }

            if (request.Image is not null)
            {
                await UpdateRouteImage(
                    route,
                    image: request.Image,
                    cancellationToken);
            }

            await _routeRepository.UpdateAsync(route);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        private async Task UpdateRouteImage(
            Route route,
            IFormFile image,
            CancellationToken cancellationToken)
        {
            var newImageUri = await _imageService.UploadImageAsync(
                image,
                cancellationToken);

            if (route.ImageUri is not null)
            {
                try
                {
                    await _imageService.DeleteAsync(
                        route.ImageUri,
                        cancellationToken);
                }
                catch (Exception)
                {
                    await _imageService.DeleteAsync(
                        newImageUri,
                        cancellationToken);

                    throw;
                }
            }

            route.SetImageUri(newImageUri);
        }

        private async Task<Result> UpdateRouteServiceCenter(
            Route route,
            ServiceCenter serviceCenter)
        {
            var setResult = route.SetServiceCenter(serviceCenter);

            if (setResult.IsFailure)
            {
                return setResult.Error;
            }

            var addResult = serviceCenter.AddRoute(route);

            if (addResult.IsFailure)
            {
                return addResult.Error;
            }

            await _serviceCenterRepository.UpdateAsync(serviceCenter);

            return Result.Success();
        }

        private static Result UpdateExistingRoute(Route route, Route newRoute)
        {
            return route.Update(
                routeNumber: newRoute.RouteNumber,
                routeDescription: newRoute.Description,
                imageUri: route.ImageUri);
        }
    }
}
