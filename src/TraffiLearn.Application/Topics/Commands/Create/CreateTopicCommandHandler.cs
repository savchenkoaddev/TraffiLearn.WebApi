using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Domain.Aggregates.Topics;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Topics.Commands.Create
{
    internal sealed class CreateTopicCommandHandler
        : IRequestHandler<CreateTopicCommand, Result<Guid>>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IImageService _imageService;
        private readonly Mapper<CreateTopicCommand, Result<Topic>> _topicMapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTopicCommandHandler(
            ITopicRepository topicRepository,
            IImageService imageService,
            Mapper<CreateTopicCommand, Result<Topic>> topicMapper,
            IUnitOfWork unitOfWork)
        {
            _topicRepository = topicRepository;
            _imageService = imageService;
            _topicMapper = topicMapper;
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

            if (request.Image is not null)
            {
                var imageUri = await _imageService.UploadImageAsync(
                    request.Image, cancellationToken);

                topic.SetImageUri(imageUri);
            }

            await _topicRepository.InsertAsync(
                topic,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(topic.Id.Value);
        }
    }
}
