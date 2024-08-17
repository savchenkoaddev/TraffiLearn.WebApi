using MediatR;
using Microsoft.AspNetCore.Http;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Topics;
using TraffiLearn.Domain.Aggregates.Topics.Errors;
using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Questions.Commands.Create
{
    internal sealed class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, Result>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly IBlobService _blobService;
        private readonly Mapper<CreateQuestionCommand, Result<Question>> _questionMapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateQuestionCommandHandler(
            IQuestionRepository questionRepository,
            ITopicRepository topicRepository,
            IBlobService blobService,
            Mapper<CreateQuestionCommand, Result<Question>> questionMapper,
            IUnitOfWork unitOfWork)
        {
            _questionRepository = questionRepository;
            _topicRepository = topicRepository;
            _blobService = blobService;
            _questionMapper = questionMapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            CreateQuestionCommand request,
            CancellationToken cancellationToken)
        {
            var mappingResult = _questionMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return mappingResult.Error;
            }

            var question = mappingResult.Value;

            var addResult = await HandleTopics(
                question,
                topicsIds: request.TopicIds,
                cancellationToken);

            if (addResult.IsFailure)
            {
                return addResult.Error;
            }

            var imageResult = await HandleImage(
                question,
                image: request.Image,
                cancellationToken);

            if (imageResult.IsFailure)
            {
                return imageResult.Error;
            }

            await _questionRepository.AddAsync(
                question,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        private async Task<Result> HandleTopics(
            Question question,
            List<Guid>? topicsIds,
            CancellationToken cancellationToken = default)
        {
            foreach (var topicId in topicsIds)
            {
                var topic = await _topicRepository.GetByIdAsync(
                    topicId: new TopicId(topicId),
                    cancellationToken);

                if (topic is null)
                {
                    return TopicErrors.NotFound;
                }

                var topicAddResult = question.AddTopic(topic.Id);

                if (topicAddResult.IsFailure)
                {
                    return topicAddResult.Error;
                }
            }

            return Result.Success();
        }

        private async Task<Result> HandleImage(
            Question question,
            IFormFile? image,
            CancellationToken cancellationToken)
        {
            if (image is not null)
            {
                using Stream stream = image.OpenReadStream();

                var uploadResponse = await _blobService.UploadAsync(
                    stream,
                    contentType: image.ContentType,
                    cancellationToken);

                Result<ImageUri> imageUriResult = ImageUri.Create(uploadResponse.BlobUri);

                if (imageUriResult.IsFailure)
                {
                    return imageUriResult.Error;
                }

                question.SetImageUri(imageUriResult.Value);
            }

            return Result.Success();
        }
    }
}
