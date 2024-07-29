using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Topics;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Application.Commands.Questions.Create
{
    public sealed class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, Result>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlobService _blobService;
        private readonly Mapper<CreateQuestionCommand, Result<Question>> _questionMapper;

        public CreateQuestionCommandHandler(
            IQuestionRepository questionRepository,
            ITopicRepository topicRepository,
            IUnitOfWork unitOfWork,
            IBlobService blobService,
            Mapper<CreateQuestionCommand, Result<Question>> questionMapper)
        {
            _questionRepository = questionRepository;
            _topicRepository = topicRepository;
            _unitOfWork = unitOfWork;
            _blobService = blobService;
            _questionMapper = questionMapper;
        }

        public async Task<Result> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
        {
            var questionResult = _questionMapper.Map(request);

            if (questionResult.IsFailure)
            {
                return questionResult.Error;
            }

            var question = questionResult.Value;

            foreach (var topicId in request.TopicsIds)
            {
                var topic = await _topicRepository.GetByIdAsync(topicId.Value);

                if (topic is null)
                {
                    return TopicErrors.NotFound;
                }

                question.AddTopic(topic);
            }

            var image = request.Image;

            if (image is not null)
            {
                using Stream stream = image.OpenReadStream();

                var uploadResponse = await _blobService.UploadAsync(
                    stream,
                    image.ContentType,
                    cancellationToken);

                Result<ImageUri> imageUriResult = ImageUri.Create(uploadResponse.BlobUri);

                if (imageUriResult.IsFailure)
                {
                    return imageUriResult.Error;
                }

                question.SetImageUri(imageUriResult.Value);
            }

            await _questionRepository.AddAsync(question);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
