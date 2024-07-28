using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Application.Commands.Questions.Create
{
    public sealed class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlobService _blobService;
        private readonly Mapper<CreateQuestionCommand, Question> _questionMapper;

        public CreateQuestionCommandHandler(
            IQuestionRepository questionRepository,
            ITopicRepository topicRepository,
            IUnitOfWork unitOfWork,
            IBlobService blobService,
            Mapper<CreateQuestionCommand, Question> questionMapper)
        {
            _questionRepository = questionRepository;
            _topicRepository = topicRepository;
            _unitOfWork = unitOfWork;
            _blobService = blobService;
            _questionMapper = questionMapper;
        }

        public async Task Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = _questionMapper.Map(request);

            foreach (var topicId in request.TopicsIds)
            {
                var topic = await _topicRepository.GetByIdAsync(topicId.Value);

                if (topic is null)
                {
                    throw new ArgumentException("Topic has not been found");
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

                ImageUri imageUri = ImageUri.Create(uploadResponse.BlobUri);

                question.SetImageUri(imageUri);
            }

            await _questionRepository.AddAsync(question);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
