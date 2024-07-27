using MediatR;
using Microsoft.AspNetCore.Http;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Application.Questions.Commands.UpdateQuestion
{
    public sealed class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand>
    {
        private readonly IBlobService _blobService;
        private readonly IQuestionRepository _questionRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly IUnitOfWork _unitOfWork;

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
            var question = await _questionRepository.GetByIdAsync(
                request.QuestionId,
                includeExpression: x => x.Topics);

            if (question is null)
            {
                throw new QuestionNotFoundException(request.QuestionId);
            }

            var answers = request.RequestObject.Answers
                .Select(a => Answer.Create(
                    a.Text,
                    a.IsCorrect))
                .ToList();

            question.Update(
                content: request.RequestObject.Content,
                explanation: request.RequestObject.Explanation,
                questionTitleDetails: QuestionTitleDetails.Create(
                    ticketNumber: request.RequestObject.TitleDetails.TicketNumber,
                    questionNumber: request.RequestObject.TitleDetails.QuestionNumber),
                answers: answers);

            await UpdateTopics(
                topicsIds: request.RequestObject.TopicsIds,
                question: question);

            await HandleImageAsync(
                image: request.Image,
                question,
                removeOldImageIfNewImageMissing: request.RequestObject.RemoveOldImageIfNewImageMissing,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task UpdateTopics(
            IEnumerable<Guid> topicsIds,
            Question question)
        {
            foreach (var topicId in topicsIds)
            {
                var topic = await _topicRepository.GetByIdAsync(topicId);

                if (topic is null)
                {
                    throw new ArgumentException($"Topic with the {topicId} id has not been found.");
                }

                if (!question.Topics.Contains(topic))
                {
                    question.AddTopic(topic);
                }
            }

            foreach (var topic in question.Topics)
            {
                if (!topicsIds.Contains(topic.Id))
                {
                    question.RemoveTopic(topic);
                }
            }
        }

        private async Task HandleImageAsync(
            IFormFile? image,
            Question question,
            bool removeOldImageIfNewImageMissing,
            CancellationToken cancellationToken)
        {
            if (image is null)
            {
                if (question.ImageUri is not null && removeOldImageIfNewImageMissing)
                {
                    await _blobService.DeleteAsync(question.ImageUri, cancellationToken);

                    question.SetImageUri(null);
                }
            }
            else
            {
                if (question.ImageUri is not null)
                {
                    await _blobService.DeleteAsync(question.ImageUri, cancellationToken);
                }

                using Stream stream = image.OpenReadStream();

                var uploadResponse = await _blobService.UploadAsync(
                    stream,
                    image.ContentType,
                    cancellationToken);

                question.SetImageUri(uploadResponse.BlobUri);
            }
        }
    }
}
