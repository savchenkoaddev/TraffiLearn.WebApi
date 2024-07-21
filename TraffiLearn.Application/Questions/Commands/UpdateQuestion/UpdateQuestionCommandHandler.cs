using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Questions.Commands.UpdateQuestion
{
    public sealed class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand>
    {
        private readonly IBlobService _blobService;
        private readonly IQuestionRepository _questionRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly QuestionMapper _questionMapper = new();

        public UpdateQuestionCommandHandler(
            IQuestionRepository questionRepository,
            ITopicRepository topicRepository,
            IUnitOfWork unitOfWork,
            IBlobService blobService)
        {
            _questionRepository = questionRepository;
            _topicRepository = topicRepository;
            _unitOfWork = unitOfWork;
            _blobService = blobService;
        }

        public async Task Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            if (request.RequestObject.Answers.All(a => a.IsCorrect == false))
            {
                throw new AllAnswersAreIncorrectException();
            }

            var oldEntityObject = await GetOldEntityObject(request.QuestionId.Value);

            var newEntityObject = _questionMapper.ToEntity(request.RequestObject);
            newEntityObject.Id = oldEntityObject.Id;

            UpdateAnswers(oldEntityObject, newEntityObject);

            await UpdateTopics(request.RequestObject.TopicsIds, oldEntityObject);

            await HandleImageAsync(request, oldEntityObject, newEntityObject, cancellationToken);

            await _questionRepository.UpdateAsync(oldEntityObject, newEntityObject);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task<Question> GetOldEntityObject(Guid questionId)
        {
            var oldEntityObject = await _questionRepository.GetByIdAsync(questionId, includeExpression: x => x.Topics);

            if (oldEntityObject is null)
            {
                throw new QuestionNotFoundException(questionId);
            }

            return oldEntityObject;
        }

        private void UpdateAnswers(
            Question oldEntityObject, 
            Question newEntityObject)
        {
            oldEntityObject.Answers = newEntityObject.Answers;
        }

        private async Task UpdateTopics(
            IEnumerable<Guid?>? topicsIds, 
            Question oldEntityObject)
        {
            foreach (var topicId in topicsIds)
            {
                var topic = await _topicRepository.GetByIdAsync(topicId.Value);

                if (topic is null)
                {
                    throw new TopicNotFoundException(topicId.Value);
                }

                if (!oldEntityObject.Topics.Any(x => x.Id == topic.Id))
                {
                    oldEntityObject.Topics.Add(topic);
                }
            }

            oldEntityObject.Topics = oldEntityObject.Topics.IntersectBy(topicsIds, x => x.Id).ToList();
        }

        private async Task HandleImageAsync(
            UpdateQuestionCommand request,
            Question oldEntityObject,
            Question newEntityObject,
            CancellationToken cancellationToken)
        {
            var image = request.Image;

            if (image is not null)
            {
                if (oldEntityObject.ImageUri is not null)
                {
                    await _blobService.DeleteAsync(oldEntityObject.ImageUri, cancellationToken);
                }

                using Stream stream = image.OpenReadStream();

                var uploadResponse = await _blobService.UploadAsync(stream, image.ContentType, cancellationToken);

                newEntityObject.ImageUri = uploadResponse.BlobUri;
            }
            else
            {
                if (oldEntityObject.ImageUri is not null && request.RequestObject.RemoveOldImageIfNewImageMissing)
                {
                    await _blobService.DeleteAsync(oldEntityObject.ImageUri, cancellationToken);
                }
                else if (oldEntityObject.ImageUri is not null)
                {
                    newEntityObject.ImageUri = oldEntityObject.ImageUri;
                }
            }
        }
    }
}
