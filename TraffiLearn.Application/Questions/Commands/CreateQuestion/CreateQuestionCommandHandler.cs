using MediatR;
using System.IO;
using TraffiLearn.Application.Abstractions;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Application.DTO.Questions.Request;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Questions.Commands.CreateQuestion
{
    public sealed class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlobService _blobService;
        private readonly IMapper<QuestionCreateRequest, Question> _questionMapper;

        public CreateQuestionCommandHandler(
            IQuestionRepository questionRepository,
            ITopicRepository topicRepository,
            IUnitOfWork unitOfWork,
            IBlobService blobService,
            IMapper<QuestionCreateRequest, Question> questionMapper)
        {
            _questionRepository = questionRepository;
            _topicRepository = topicRepository;
            _unitOfWork = unitOfWork;
            _blobService = blobService;
            _questionMapper = questionMapper;
        }

        public async Task Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
        {
            if (request.RequestObject.Answers.All(a => a.IsCorrect == false))
            {
                throw new AllAnswersAreIncorrectException();
            }

            var entity = _questionMapper.Map(request.RequestObject);

            foreach (var topicId in request.RequestObject.TopicsIds)
            {
                var topic = await _topicRepository.GetByIdAsync(topicId.Value);

                if (topic is null)
                {
                    throw new TopicNotFoundException(topicId.Value);
                }

                entity.Topics.Add(topic);
            }

            var image = request.Image;

            if (image is not null)
            {
                using Stream stream = image.OpenReadStream();

                var uploadResponse = await _blobService.UploadAsync(
                    stream,
                    image.ContentType, 
                    cancellationToken);

                entity.ImageUri = uploadResponse.BlobUri;
            }

            await _questionRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
