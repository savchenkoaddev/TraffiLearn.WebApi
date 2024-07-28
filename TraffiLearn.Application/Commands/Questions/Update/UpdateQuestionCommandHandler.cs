using MediatR;
using Microsoft.AspNetCore.Http;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Application.Commands.Questions.Update
{
    public sealed class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly IBlobService _blobService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateQuestionCommandHandler(
            IQuestionRepository questionRepository,
            ITopicRepository topicRepository,
            IBlobService blobService,
            IUnitOfWork unitOfWork)
        {
            _questionRepository = questionRepository;
            _topicRepository = topicRepository;
            _blobService = blobService;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdAsync(
                request.QuestionId.Value,
                includeExpression: x => x.Topics);

            if (question is null)
            {
                throw new ArgumentException("Question has not been found");
            }

            var answers = request.Answers.Select(x => Answer.Create(x.Text, x.IsCorrect.Value)).ToList();

            question.Update(
                content: QuestionContent.Create(request.Content),
                explanation: QuestionExplanation.Create(request.Explanation),
                ticketNumber: TicketNumber.Create(request.TicketNumber.Value),
                questionNumber: QuestionNumber.Create(request.QuestionNumber.Value),
                answers: answers,
                imageUri: question.ImageUri);

            await UpdateTopics(
                topicsIds: request.TopicsIds,
                question: question);

            await _questionRepository.UpdateAsync(question);

            await HandleImageAsync(
                image: request.Image,
                question,
                request.RemoveOldImageIfNewImageMissing.Value,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task UpdateTopics(
            List<Guid?>? topicsIds,
            Question question)
        {
            foreach (var topicId in topicsIds)
            {
                var topic = await _topicRepository.GetByIdAsync(topicId.Value);

                if (topic is null)
                {
                    throw new ArgumentException($"Topic with the {topicId} id has not been found.");
                }

                if (!question.Topics.Contains(topic))
                {
                    question.AddTopic(topic);
                }
            }

            var questionTopics = question.Topics.ToList();

            foreach (var topic in questionTopics)
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
                    await _blobService.DeleteAsync(
                        blobUri: question.ImageUri.Value,
                        cancellationToken);

                    question.SetImageUri(null);
                }
            }
            else
            {
                if (question.ImageUri is not null)
                {
                    await _blobService.DeleteAsync(
                        blobUri: question.ImageUri.Value,
                        cancellationToken);
                }

                using Stream stream = image.OpenReadStream();

                var uploadResponse = await _blobService.UploadAsync(
                    stream,
                    image.ContentType,
                    cancellationToken);

                var imageUri = ImageUri.Create(uploadResponse.BlobUri);

                question.SetImageUri(imageUri);
            }
        }
    }
}
