using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.Errors;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Topics;
using TraffiLearn.Domain.Aggregates.Topics.Errors;
using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Questions.Commands.Update
{
    internal sealed class UpdateQuestionCommandHandler
        : IRequestHandler<UpdateQuestionCommand, Result>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly IImageService _imageService;
        private readonly Mapper<UpdateQuestionCommand, Result<Question>> _commandMapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateQuestionCommandHandler> _logger;

        public UpdateQuestionCommandHandler(
            IQuestionRepository questionRepository,
            ITopicRepository topicRepository,
            IImageService imageService,
            Mapper<UpdateQuestionCommand, Result<Question>> commandMapper,
            IUnitOfWork unitOfWork,
            ILogger<UpdateQuestionCommandHandler> logger)
        {
            _questionRepository = questionRepository;
            _topicRepository = topicRepository;
            _imageService = imageService;
            _commandMapper = commandMapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(
            UpdateQuestionCommand request,
            CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdWithTopicsAsync(
                questionId: new QuestionId(request.QuestionId.Value),
                cancellationToken);

            if (question is null)
            {
                return QuestionErrors.NotFound;
            }

            var mappingResult = _commandMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return mappingResult.Error;
            }

            var newQuestion = mappingResult.Value;

            var updateResult = question.Update(
                newQuestion.Content,
                newQuestion.Explanation,
                newQuestion.QuestionNumber,
                newQuestion.Answers.ToList(),
                imageUri: question.ImageUri);

            if (updateResult.IsFailure)
            {
                return updateResult.Error;
            }

            var updateTopicsResult = await UpdateTopics(
                topicIds: request.TopicIds,
                question,
                cancellationToken);

            if (updateTopicsResult.IsFailure)
            {
                return updateTopicsResult.Error;
            }

            await _questionRepository.UpdateAsync(question);

            var removeOldImageIfNewImageMissing =
                request.RemoveOldImageIfNewImageMissing!.Value;

            var imageUpdateResult = await HandleImageAsync(
                image: request.Image,
                question,
                removeOldImageIfNewImageMissing,
                cancellationToken);

            if (imageUpdateResult.IsFailure)
            {
                return imageUpdateResult.Error;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        private async Task<Result> UpdateTopics(
            List<Guid>? topicIds,
            Question question,
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

                if (!question.Topics.Contains(topic))
                {
                    var topicAddResult = question.AddTopic(topic);

                    if (topicAddResult.IsFailure)
                    {
                        return topicAddResult.Error;
                    }
                }
            }

            var questionTopics = question.Topics.ToList();

            foreach (var topic in questionTopics)
            {
                if (!topicIds.Contains(topic.Id.Value))
                {
                    var topicRemoveResult = question.RemoveTopic(topic);

                    if (topicRemoveResult.IsFailure)
                    {
                        return topicRemoveResult.Error;
                    }
                }
            }

            return Result.Success();
        }

        private async Task<Result> HandleImageAsync(
            IFormFile? image,
            Question question,
            bool removeOldImageIfNewImageMissing,
            CancellationToken cancellationToken = default)
        {
            if (image is null)
            {
                if (question.ImageUri is not null && removeOldImageIfNewImageMissing)
                {
                    await _imageService.DeleteAsync(
                        question.ImageUri,
                        cancellationToken);

                    question.SetImageUri(null);
                }
            }
            else
            {
                if (question.ImageUri is not null)
                {
                    await _imageService.DeleteAsync(
                        question.ImageUri,
                        cancellationToken);
                }

                var imageUri = await _imageService.UploadImageAsync(
                    image,
                    cancellationToken);

                question.SetImageUri(imageUri);
            }

            return Result.Success();
        }
    }
}