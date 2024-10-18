using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Domain.Aggregates.Common.ImageUri;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Topics;
using TraffiLearn.Domain.Aggregates.Topics.Errors;
using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Questions.Commands.Create
{
    internal sealed class CreateQuestionCommandHandler
        : IRequestHandler<CreateQuestionCommand, Result<Guid>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly IImageService _imageService;
        private readonly Mapper<CreateQuestionCommand, Result<Question>> _questionMapper;
        private readonly ILogger<CreateQuestionCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CreateQuestionCommandHandler(
            IQuestionRepository questionRepository,
            ITopicRepository topicRepository,
            IImageService imageService,
            Mapper<CreateQuestionCommand, Result<Question>> questionMapper,
            ILogger<CreateQuestionCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _questionRepository = questionRepository;
            _topicRepository = topicRepository;
            _imageService = imageService;
            _questionMapper = questionMapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            CreateQuestionCommand request,
            CancellationToken cancellationToken)
        {
            var mappingResult = _questionMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return Result.Failure<Guid>(mappingResult.Error);
            }

            var question = mappingResult.Value;

            var addResult = await HandleTopics(
                question,
                topicIds: request.TopicIds,
                cancellationToken);

            if (addResult.IsFailure)
            {
                return Result.Failure<Guid>(addResult.Error);
            }

            ImageUri? newImageUri = null;

            if (ImageIsProvidedIn(request))
            {
                newImageUri = await _imageService.UploadImageAsync(
                    image: request.Image,
                    cancellationToken);
            }

            try
            {
                question.SetImageUri(newImageUri);

                await _questionRepository.InsertAsync(
                    question,
                    cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception)
            {
                if (newImageUri is not null)
                {
                    await _imageService.DeleteAsync(
                        imageUri: newImageUri,
                        cancellationToken);

                    _logger.LogInformation(
                        "Succesfully deleted the uploaded image during the error. The image uri: {imageUri}", newImageUri.Value);
                }

                throw;
            }

            return Result.Success(question.Id.Value);
        }

        private bool ImageIsProvidedIn(CreateQuestionCommand request)
        {
            return request.Image is not null;
        }

        private async Task<Result> HandleTopics(
            Question question,
            List<Guid>? topicIds,
            CancellationToken cancellationToken = default)
        {
            foreach (var topicId in topicIds)
            {
                var topic = await _topicRepository.GetByIdAsync(
                    topicId: new TopicId(topicId),
                    cancellationToken);

                if (topic is null)
                {
                    return TopicErrors.NotFound;
                }

                var topicAddResult = question.AddTopic(topic);

                if (topicAddResult.IsFailure)
                {
                    return Error.InternalFailure();
                }
            }

            return Result.Success();
        }
    }
}
