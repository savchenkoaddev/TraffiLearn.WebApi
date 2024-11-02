using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Domain.Topics;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Topics.Commands.Update
{
    internal sealed class UpdateTopicCommandHandler : IRequestHandler<UpdateTopicCommand, Result>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IImageService _imageService;
        private readonly Mapper<UpdateTopicCommand, Result<Topic>> _commandMapper;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTopicCommandHandler(
            ITopicRepository topicRepository,
            IImageService imageService,
            Mapper<UpdateTopicCommand, Result<Topic>> commandMapper,
            IUnitOfWork unitOfWork)
        {
            _topicRepository = topicRepository;
            _imageService = imageService;
            _commandMapper = commandMapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            UpdateTopicCommand request,
            CancellationToken cancellationToken)
        {
            var topic = await _topicRepository.GetByIdAsync(
                topicId: new TopicId(request.TopicId.Value),
                cancellationToken);

            if (topic is null)
            {
                return TopicErrors.NotFound;
            }

            var mappingResult = _commandMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return mappingResult.Error;
            }

            await HandleImageUpdateAsync(
                topic, request, cancellationToken);

            Result updateResult = topic.Update(
                topicNumber: mappingResult.Value.Number,
                topicTitle: mappingResult.Value.Title);

            if (updateResult.IsFailure)
            {
                return updateResult.Error;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        private async Task HandleImageUpdateAsync(
            Topic topic,
            UpdateTopicCommand request,
            CancellationToken cancellationToken = default)
        {
            if (request.Image is not null)
            {
                var newImageUri = await _imageService.UploadImageAsync(
                    request.Image, cancellationToken);

                if (topic.ImageUri is not null)
                {
                    try
                    {
                        await _imageService.DeleteAsync(
                            topic.ImageUri, cancellationToken);
                    }
                    catch (Exception)
                    {
                        await _imageService.DeleteAsync(
                            topic.ImageUri, cancellationToken);

                        throw;
                    }
                }

                topic.SetImageUri(newImageUri);
            }
            else if (request.RemoveOldImageIfNewMissing
                && topic.ImageUri is not null)
            {
                await _imageService.DeleteAsync(
                    topic.ImageUri, cancellationToken);

                topic.SetImageUri(null);
            }
        }
    }
}
