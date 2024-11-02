using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Domain.Topics;
using TraffiLearn.SharedKernel.Shared;
using TraffiLearn.SharedKernel.ValueObjects.ImageUris;

namespace TraffiLearn.Application.Topics.Commands.Create
{
    internal sealed class CreateTopicCommandHandler
        : IRequestHandler<CreateTopicCommand, Result<Guid>>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IImageService _imageService;
        private readonly Mapper<CreateTopicCommand, Result<Topic>> _topicMapper;
        private readonly ILogger<CreateTopicCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTopicCommandHandler(
            ITopicRepository topicRepository,
            IImageService imageService,
            Mapper<CreateTopicCommand, Result<Topic>> topicMapper,
            ILogger<CreateTopicCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _topicRepository = topicRepository;
            _imageService = imageService;
            _topicMapper = topicMapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            CreateTopicCommand request,
            CancellationToken cancellationToken)
        {
            var mappingResult = _topicMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return Result.Failure<Guid>(mappingResult.Error);
            }

            var topic = mappingResult.Value;

            ImageUri? imageUri = null;

            if (request.Image is not null)
            {
                imageUri = await _imageService.UploadImageAsync(
                    request.Image, cancellationToken);
            }

            try
            {
                topic.SetImageUri(imageUri);

                await _topicRepository.InsertAsync(
                    topic,
                    cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception)
            {
                if (imageUri is not null)
                {
                    await _imageService.DeleteAsync(
                        imageUri: imageUri,
                        cancellationToken);

                    _logger.LogInformation(
                        "Succesfully deleted the uploaded image during the error. Image uri: {imageUri}",
                        imageUri.Value);
                }

                throw;
            }

            return Result.Success(topic.Id.Value);
        }
    }
}
