using MediatR;
using Microsoft.AspNetCore.Http;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Application.Storage.Blobs.DTO;
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

            if (ImageIsProvidedIn(request))
            {
                var uploadResponse = await UploadImage(
                    image: request.Image,
                    cancellationToken);

                var assignResult = AssignImageUriToQuestion(
                    question,
                    uploadResponse);

                if (assignResult.IsFailure)
                {
                    return Result.Failure<Guid>(assignResult.Error);
                }
            }

            await _questionRepository.InsertAsync(
                question,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

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

        private async Task<UploadBlobResponse> UploadImage(
            IFormFile? image,
            CancellationToken cancellationToken)
        {
            using Stream stream = image.OpenReadStream();

            var uploadResponse = await _blobService.UploadAsync(
                stream,
                contentType: image.ContentType,
                cancellationToken);

            return uploadResponse;
        }

        private static Result AssignImageUriToQuestion(
            Question question,
            UploadBlobResponse uploadResponse)
        {
            Result<ImageUri> imageUriResult = ImageUri.Create(uploadResponse.BlobUri);

            if (imageUriResult.IsFailure)
            {
                return imageUriResult.Error;
            }

            question.SetImageUri(imageUriResult.Value);

            return Result.Success();
        }
    }
}
